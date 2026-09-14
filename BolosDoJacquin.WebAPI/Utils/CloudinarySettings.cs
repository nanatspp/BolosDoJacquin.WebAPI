namespace BolosDoJacquin.WebAPI.Utils
{
    public class CloudinarySettings
    {
        //nome da conta no cloudinary
        public string CloudName { get; set; } = string.Empty;

        //chave publica de identificação da API
        public string ApiKey { get; set; } = string.Empty;

        //cheve secreta qu assina/autetica as requisições
        public string ApiSecret { get; set; } = string.Empty;
    }
}
