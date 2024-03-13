using System.Runtime.InteropServices;

namespace EWS.Extensions
{
    public static class BinarySerializationExtensions
    {
        /// <summary>
        /// Converts the given object to a cross platform byte array to be networked.
        /// </summary>
        public static byte[] ToNetworkByteArray<T>(this T obj) where T : unmanaged
        {
            var size = Marshal.SizeOf(obj);
            var arr = new byte[size];

            var ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(obj, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);

            return arr;
        }

        /// <summary>
        /// Converts the given byte array to a cross platform object to be used in the application.
        /// </summary>
        public static T FromNetworkByteArray<T>(this byte[] arr) where T : unmanaged
        {
            var size = Marshal.SizeOf<T>();
            if (size > arr.Length) return default;

            var ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(arr, 0, ptr, size);

            var obj = Marshal.PtrToStructure<T>(ptr);
            Marshal.FreeHGlobal(ptr);

            return obj;
        }
    }
}