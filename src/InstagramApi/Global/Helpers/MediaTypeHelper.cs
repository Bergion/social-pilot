using InstagramApi.Global.Enums;

namespace InstagramApi.Global.Helpers
{
    public static class MediaTypeHelper
    {
        private static string[] videoExtensions = { "MOV", "MP4" };
        private static string[] imageExtensions = { "JPEG", "JPG" };

        public static MediaType GetMediaType(string path)
        {
            if (videoExtensions.Contains(Path.GetExtension(path), StringComparer.OrdinalIgnoreCase))
            {
                return MediaType.Video;
            }

            if (imageExtensions.Contains(Path.GetExtension(path), StringComparer.OrdinalIgnoreCase))
            {
                return MediaType.Image;
            }

            return MediaType.Other;
        }
    }
}
