using System;

namespace Kin.Sdk
{
    public static class Utils
    {
        /// <summary>
        /// Extenstion method to throw ArgumentNullExcpetion if object is null
        /// </summary>
        /// <param name="obj">Object to check</param>
        /// <param name="name">Name of object to include in the excpetion</param>
        public static void ThrowIfNull(this object obj, string name)
        {
            if (obj == null)
                throw new ArgumentNullException(name);
        }
    }
}
