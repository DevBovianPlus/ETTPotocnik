using DevExpress.Web;
using DevExpress.Web.Internal;
using LokacijaWeb.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ETT_Web.Widgets
{
    public partial class ImageUploadWidget : System.Web.UI.UserControl
    {
        const string UploadDirectory = "~/UploadControl/UploadImages/";
        const string UploadDirectoryProfileImages = "~/UploadControl/Profile/";
        const string UploadDirectoryCompanyImages = "~/UploadControl/Company/";

        public event EventHandler ImageUpdated;
        public HtmlImage ProfileImage { get { return uploadedImage; } }
        public string ImageFullFileName { get; set; }
        public string ImageFileName { get; set; }

        private int thumbnailwidth = 80;
        private int thumbnailheight = 80;
        private bool isThumbnail = true;

        private int width = 250;
        private int height = 250;

        private string imageType;

        public string ImageType
        {
            get { return imageType; }
            set { imageType = value; }
        }

        public int ThumbnailWidth
        {
            get { return thumbnailwidth; }
            set { thumbnailwidth = value; }
        }

        public int ThumbnailHeight
        {
            get { return thumbnailheight; }
            set { thumbnailheight = value; }
        }

        public bool IsThumbnail
        {
            get { return isThumbnail; }
            set { isThumbnail = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ProfileImage.Attributes["onload"] = "onImageLoad()";

            if (!IsThumbnail)
            {
                externalDropZone.Style.Add("width", Width.ToString() + "px");
                externalDropZone.Style.Add("height", Height.ToString() + "px");
            }
        }

        protected void UploadControl_FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
        {
            e.CallbackData = SavePostedFile(e.UploadedFile);
            ImageUpdated(this, EventArgs.Empty);
        }
        protected string SavePostedFile(UploadedFile uploadedFile)
        {
            if (!uploadedFile.IsValid)
                return string.Empty;

            string fileName = Path.ChangeExtension(Path.GetRandomFileName(), ".png");
            string fullFileName = "";

            switch (ImageType)
            {
                case "Profile":
                    fullFileName = CombinePath(fileName, UploadDirectoryProfileImages);                    
                    break;
                case "Company":
                    fullFileName = CombinePath(fileName, UploadDirectoryCompanyImages);
                    break;
            }

            using (System.Drawing.Image original = System.Drawing.Image.FromStream(uploadedFile.FileContent))
            {
                if (IsThumbnail)
                {
                    using (System.Drawing.Image thumbnail = new ImageThumbnailCreator(original).CreateImageThumbnail(new Size(ThumbnailWidth, ThumbnailHeight)))
                        ImageUtils.SaveImage((Bitmap)thumbnail, fullFileName);
                }
                else
                {
                    ImageUtils.SaveImage((Bitmap)original, fullFileName.Insert(fullFileName.IndexOf(".png"), "_original"));
                    
                    System.Drawing.Image resizedPic = PictureHelper.ResizePicture(original, Width, Height);
                    ImageUtils.SaveImage((Bitmap)resizedPic, fullFileName);
                }

            }


            ImageFullFileName = fullFileName;
            ImageFileName = fileName;
            ImageFullFileName =ImageFullFileName.Replace(AppDomain.CurrentDomain.BaseDirectory, "/").Replace("\\", "/");
            
            return ImageFullFileName;
        }
        protected string CombinePath(string fileName, string directory)
        {
            string filePath = Path.Combine(Server.MapPath(directory), fileName);

            if (!Directory.Exists(Server.MapPath(directory)))
                Directory.CreateDirectory(Server.MapPath(directory));

            return filePath;
        }

        protected void ClearImageCallback_Callback(object source, CallbackEventArgs e)
        {
            ImageFullFileName = "";
            ImageFileName = "";
            ImageUpdated(this, EventArgs.Empty);
        }
    }
}