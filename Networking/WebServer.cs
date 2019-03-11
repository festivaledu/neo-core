﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Neo.Core.Shared;

namespace Neo.Core.Networking
{
    public class WebServer
    {
        public int Port { get; private set; }

        private static readonly IDictionary<string, string> mimeTypeMappings = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase) {
            #region Extension to MIME type list
            { ".asf", "video/x-ms-asf" },
            { ".asx", "video/x-ms-asf" },
            { ".avi", "video/x-msvideo" },
            { ".bin", "application/octet-stream" },
            { ".cco", "application/x-cocoa" },
            { ".crt", "application/x-x509-ca-cert" },
            { ".css", "text/css" },
            { ".deb", "application/octet-stream" },
            { ".der", "application/x-x509-ca-cert" },
            { ".dll", "application/octet-stream" },
            { ".dmg", "application/octet-stream" },
            { ".ear", "application/java-archive" },
            { ".eot", "application/octet-stream" },
            { ".exe", "application/octet-stream" },
            { ".flv", "video/x-flv" },
            { ".gif", "image/gif" },
            { ".hqx", "application/mac-binhex40" },
            { ".htc", "text/x-component" },
            { ".htm", "text/html" },
            { ".html", "text/html" },
            { ".ico", "image/x-icon" },
            { ".img", "application/octet-stream" },
            { ".iso", "application/octet-stream" },
            { ".jar", "application/java-archive" },
            { ".jardiff", "application/x-java-archive-diff" },
            { ".jng", "image/x-jng" },
            { ".jnlp", "application/x-java-jnlp-file" },
            { ".jpeg", "image/jpeg" },
            { ".jpg", "image/jpeg" },
            { ".js", "application/x-javascript" },
            { ".mml", "text/mathml" },
            { ".mng", "video/x-mng" },
            { ".mov", "video/quicktime" },
            { ".mp3", "audio/mpeg" },
            { ".mpeg", "video/mpeg" },
            { ".mpg", "video/mpeg" },
            { ".msi", "application/octet-stream" },
            { ".msm", "application/octet-stream" },
            { ".msp", "application/octet-stream" },
            { ".pdb", "application/x-pilot" },
            { ".pdf", "application/pdf" },
            { ".pem", "application/x-x509-ca-cert" },
            { ".pl", "application/x-perl" },
            { ".pm", "application/x-perl" },
            { ".png", "image/png" },
            { ".prc", "application/x-pilot" },
            { ".ra", "audio/x-realaudio" },
            { ".rar", "application/x-rar-compressed" },
            { ".rpm", "application/x-redhat-package-manager" },
            { ".rss", "text/xml" },
            { ".run", "application/x-makeself" },
            { ".sea", "application/x-sea" },
            { ".shtml", "text/html" },
            { ".sit", "application/x-stuffit" },
            { ".swf", "application/x-shockwave-flash" },
            { ".tcl", "application/x-tcl" },
            { ".tk", "application/x-tcl" },
            { ".txt", "text/plain" },
            { ".war", "application/java-archive" },
            { ".wbmp", "image/vnd.wap.wbmp" },
            { ".wmv", "video/x-ms-wmv" },
            { ".xml", "text/xml" },
            { ".xpi", "application/x-xpinstall" },
            { ".zip", "application/zip" },
            #endregion
        };

        private readonly string[] indexFiles = {
            "index.html",
            "index.htm",
            "default.html",
            "default.htm"
        };

        private Thread thread;
        private string rootDirectory;
        private HttpListener listener;
        private CancellationTokenSource cancellationTokenSource;


        /// <summary>
        ///     Initializes a new instance of the <see cref="WebServer"/> class.
        /// </summary>
        /// <param name="path">Directory path to serve.</param>
        /// <param name="port">Port to listen on.</param>
        public WebServer(string path, int port) {
            this.Initialize(path, port);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WebServer"/> class.
        /// </summary>
        /// <param name="path">Directory path to serve.</param>
        public WebServer(string path) {
            var l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();

            var port = ((IPEndPoint) l.LocalEndpoint).Port;

            l.Stop();

            this.Initialize(path, port);
        }

        /// <summary>
        ///     Stops the server and aborts all threads.
        /// </summary>
        public void Stop() {
            cancellationTokenSource.Cancel();

            //thread.Abort();

            listener.Stop();

            cancellationTokenSource.Dispose();
        }

        private void Listen(object obj) {
            listener = new HttpListener();
            listener.Prefixes.Add($"http://*:{Port}/");

            var token = (CancellationToken) obj;

            try {
                listener.Start();
            } catch {
                Logger.Instance.Log(LogLevel.Error, "Error starting WebServer on :" + Port + ". Avatars won't be available.");
                return;
            }

            while (true) {
                if (token.IsCancellationRequested) {
                    return;
                }

                try {
                    var context = listener.GetContext();
                    Process(context);
                } catch { }
            }
        }

        private void Process(HttpListenerContext context) {
            var filename = context.Request.Url.AbsolutePath;
            filename = filename.Substring(1);

            if (string.IsNullOrEmpty(filename)) {
                foreach (var indexFile in indexFiles) {
                    if (File.Exists(Path.Combine(rootDirectory, indexFile))) {
                        filename = indexFile;
                        break;
                    }
                }
            }

            filename = Path.Combine(rootDirectory, filename);

            if (File.Exists(filename)) {
                try {
                    var input = new FileStream(filename, FileMode.Open);
                    
                    context.Response.ContentType = mimeTypeMappings.TryGetValue(Path.GetExtension(filename), out var mime) ? mime : "application/octet-stream";
                    context.Response.ContentLength64 = input.Length;
                    context.Response.AddHeader("Date", DateTime.Now.ToString("r"));
                    context.Response.AddHeader("Last-Modified", File.GetLastWriteTime(filename).ToString("r"));

                    var buffer = new byte[1024 * 16];
                    int nbytes;
                    while ((nbytes = input.Read(buffer, 0, buffer.Length)) > 0) {
                        context.Response.OutputStream.Write(buffer, 0, nbytes);
                    }

                    input.Close();

                    context.Response.StatusCode = (int) HttpStatusCode.OK;
                    context.Response.OutputStream.Flush();
                } catch {
                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                }
            } else {
                context.Response.StatusCode = (int) HttpStatusCode.NotFound;
            }

            context.Response.OutputStream.Close();
        }

        private void Initialize(string path, int port) {
            rootDirectory = path;
            Port = port;

            cancellationTokenSource = new CancellationTokenSource();
            ThreadPool.QueueUserWorkItem(Listen, cancellationTokenSource.Token);

            //thread = new Thread(Listen);
            //thread.Start();
        }
    }
}