// <copyright file="CookieTempDataProvider.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Security.Cryptography;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;

    /// <summary>
    /// A cookie based temp data provider. This replaces the default session state based temp data provider which is
    /// unsuitable for load balanced environments.
    /// <remarks>Adapted from <see href="https://github.com/brockallen/CookieTempData"/></remarks>
    /// </summary>
    internal class CookieTempDataProvider : ITempDataProvider
    {
        /// <summary>
        /// The anonymous cookie value prefix.
        /// </summary>
        private const string AnonymousCookieValuePrefix = "_";

        /// <summary>
        /// The authenticated cookie value prefix.
        /// </summary>
        private const string AuthenticatedCookieValuePrefix = ".";

        /// <summary>
        /// The cookie name.
        /// </summary>
        private const string CookieName = "TempData";

        /// <summary>
        /// The machine key purpose.
        /// </summary>
        private const string MachineKeyPurpose = "CookieTempDataProvider:{0}";

        /// <summary>
        /// The anonymous machine identifier.
        /// </summary>
        private const string Anonymous = "<anonymous>";

        /// <summary>
        /// The validation exception event handler.
        /// </summary>
        public static event EventHandler<Exception> ValidationException;

        /// <summary>
        /// Saves the temp data.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="values">The collection of values to save.</param>
        public void SaveTempData(
            ControllerContext controllerContext,
            IDictionary<string, object> values)
        {
            byte[] bytes = this.SerializeWithBinaryFormatter(values);
            bytes = this.Compress(bytes);
            string value = this.Protect(bytes, controllerContext.HttpContext);
            this.IssueCookie(controllerContext, value);
        }

        /// <summary>
        /// Loads the temp data.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <returns>
        /// The <see cref="T:IDictionary{string,object}"/>.
        /// </returns>
        public IDictionary<string, object> LoadTempData(
            ControllerContext controllerContext)
        {
            string value = this.GetCookieValue(controllerContext);
            byte[] bytes = this.Unprotect(value, controllerContext.HttpContext);
            if (bytes == null && value != null)
            {
                // Failure, so remove cookie
                this.IssueCookie(controllerContext, null);
                return null;
            }
            else
            {
                bytes = this.Decompress(bytes);
                return this.DeserializeWithBinaryFormatter(bytes);
            }
        }

        /// <summary>
        /// Gets the cookie value.
        /// </summary>
        /// <param name="controllerContext">
        /// The controller context.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string GetCookieValue(ControllerContext controllerContext)
        {
            if (controllerContext.HttpContext.Request.Cookies.AllKeys.Contains(CookieName))
            {
                HttpCookie c = controllerContext.HttpContext.Request.Cookies[CookieName];
                return c?.Value;
            }

            return null;
        }

        /// <summary>
        /// Issues a cookie to the controller context with the given value.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="value">The value.</param>
        private void IssueCookie(ControllerContext controllerContext, string value)
        {
            // if we don't have a value and there's no prior cookie then exit
            if (value == null && !controllerContext.HttpContext.Request.Cookies.AllKeys.Contains(CookieName))
            {
                return;
            }

            HttpCookie c = new HttpCookie(CookieName, value)
            {
                // don't allow javascript access to the cookie
                HttpOnly = true,

                // set the path so other apps on the same server don't see the cookie
                Path = controllerContext.HttpContext.Request.ApplicationPath,

                // ideally we're always going over SSL, but be flexible for non-SSL apps
                Secure = controllerContext.HttpContext.Request.IsSecureConnection
            };

            if (value == null)
            {
                // if we have no data then issue an expired cookie to clear the cookie
                c.Expires = DateTime.Now.AddMonths(-1);
            }

            controllerContext.HttpContext.Response.Cookies.Add(c);
        }

        /// <summary>
        /// Gets the anonymous machine key purpose.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string GetAnonMachineKeyPurpose()
        {
            return string.Format(MachineKeyPurpose, Anonymous);
        }

        /// <summary>
        /// Gets a machine key purpose.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string GetMachineKeyPurpose(HttpContextBase context)
        {
            if (context.User?.Identity == null || !context.User.Identity.IsAuthenticated)
            {
                return this.GetAnonMachineKeyPurpose();
            }

            return string.Format(MachineKeyPurpose, context.User.Identity == null ? string.Empty : context.User.Identity.Name);
        }

        /// <summary>
        /// Get machine key purpose from the givenprefix.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string GetMachineKeyPurposeFromPrefix(string prefix, HttpContextBase context)
        {
            if (prefix == AnonymousCookieValuePrefix)
            {
                return this.GetAnonMachineKeyPurpose();
            }

            if (prefix == AuthenticatedCookieValuePrefix && context.User.Identity.IsAuthenticated)
            {
                return string.Format(MachineKeyPurpose, context.User.Identity.Name);
            }

            return null;
        }

        /// <summary>
        /// Gets the machine key prefix.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string GetMachineKeyPrefix(HttpContextBase context)
        {
            if (context.User?.Identity == null)
            {
                return AnonymousCookieValuePrefix;
            }

            return context.User.Identity.IsAuthenticated ? AuthenticatedCookieValuePrefix : AnonymousCookieValuePrefix;
        }

        /// <summary>
        /// Encrypts the data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string Protect(byte[] data, HttpContextBase context)
        {
            if (data == null || data.Length == 0)
            {
                return null;
            }

            string purpose = this.GetMachineKeyPurpose(context);
            byte[] value = MachineKey.Protect(data, purpose);

            string prefix = this.GetMachineKeyPrefix(context);
            return $"{prefix}{Convert.ToBase64String(value)}";
        }

        /// <summary>
        /// Decrypts the data.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The <see cref="T:byte[]"/>.
        /// </returns>
        private byte[] Unprotect(string value, HttpContextBase context)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            string prefix = value[0].ToString();
            string purpose = this.GetMachineKeyPurposeFromPrefix(prefix, context);
            if (purpose == null)
            {
                return null;
            }

            value = value.Substring(1);
            byte[] bytes = Convert.FromBase64String(value);
            try
            {
                return MachineKey.Unprotect(bytes, purpose);
            }
            catch (CryptographicException ex)
            {
                ValidationException?.Invoke(this, ex);

                return null;
            }
        }

        /// <summary>
        /// Compresses the data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>
        /// The <see cref="T:byte[]"/>.
        /// </returns>
        private byte[] Compress(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                return null;
            }

            DeflateStream deflateStream = null;
            try
            {
                using (MemoryStream input = new MemoryStream(data))
                {
                    MemoryStream output = new MemoryStream();
                    deflateStream = new DeflateStream(output, CompressionMode.Compress);
                    input.CopyTo(deflateStream);
                    return output.ToArray();
                }
            }
            finally
            {
                deflateStream?.Dispose();
            }
        }

        /// <summary>
        /// Decompresses the data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>
        /// The <see cref="T:byte[]"/>.
        /// </returns>
        private byte[] Decompress(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                return null;
            }

            DeflateStream deflateStream = null;
            try
            {
                MemoryStream input = new MemoryStream(data);
                using (MemoryStream output = new MemoryStream())
                {
                    deflateStream = new DeflateStream(input, CompressionMode.Decompress);
                    deflateStream.CopyTo(output);
                    byte[] result = output.ToArray();
                    return result;
                }
            }
            finally
            {
                deflateStream?.Dispose();
            }
        }

        /// <summary>
        /// Serializes the data with the binary formatter.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>
        /// The <see cref="T:byte[]"/>.
        /// </returns>
        private byte[] SerializeWithBinaryFormatter(IDictionary<string, object> data)
        {
            if (data == null || data.Keys.Count == 0)
            {
                return null;
            }

            BinaryFormatter f = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                f.Serialize(ms, data);
                ms.Seek(0, SeekOrigin.Begin);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Deserializes the data with the binary formatter.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>
        /// The <see cref="T:IDictionary{string,object}"/>.
        /// </returns>
        private IDictionary<string, object> DeserializeWithBinaryFormatter(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                return null;
            }

            BinaryFormatter f = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(data))
            {
                object obj = f.Deserialize(ms);
                return obj as IDictionary<string, object>;
            }
        }
    }
}