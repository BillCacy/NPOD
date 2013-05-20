/*
 * Class to allow multiple forms to use the same instance of variables
 */
namespace NasaPicOfDay
{
    public static class GlobalVariables
    {
        private static BackgroundImage _NasaImage;

        public static BackgroundImage NasaImage
        {
            get { return _NasaImage; }
            set { _NasaImage = value; }
        }
    }
}
