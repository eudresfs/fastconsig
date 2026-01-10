using System;
using System.IO;
using CP.FastConsig.WebApplication.Auxiliar;

namespace CP.FastConsig.WebApplication
{

    public partial class ImageConversions : CustomPage
    {

        #region Constantes
        
        private const string ParametroImageData = "imageData";
        private const string ParametroRemover = "remover";

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            ProcessaWebCam();
        }

        void ProcessaWebCam()
        {

            try
            {

                string strPhoto = Request.Form[ParametroImageData];
                string remover = Request.Form[ParametroRemover];

                if (!string.IsNullOrEmpty(strPhoto))
                {

                    byte[] photo = Convert.FromBase64String(strPhoto);

                    Sessao.PathWebCamImagemTemp = string.Format(@"{0}Temp\webcam{1}.jpg", Request.PhysicalApplicationPath, Sessao.IdSessao);

                    FileStream fs = new FileStream(Sessao.PathWebCamImagemTemp, FileMode.OpenOrCreate, FileAccess.Write);
                    BinaryWriter br = new BinaryWriter(fs);

                    br.Write(photo);

                    br.Flush();
                    br.Close();

                    fs.Close();

                }
                else if (!string.IsNullOrEmpty(remover))
                {
                    File.Delete(Sessao.PathWebCamImagemTemp);
                }
                

            }
            catch (Exception Ex)
            {
            
            }

        }

    }

}