using Firebase.Storage;

namespace swp_be.Utils
{
    public class FirebaseUtils
    {
        public string Bucket { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public string Url { get; set; }

        public FirebaseUtils()
        {
        }

        public async Task<string?> UploadImage(Stream? fileStream, string folderName, string fileName)
        {
            if (fileStream == null)
                return null;

            var cancellation = new CancellationTokenSource();

            var task = new FirebaseStorage("swp-be.appspot.com")
                .Child("images")
				.Child(folderName)
                .Child(fileName)
                .PutAsync(fileStream);

            try
            {
                var downloadUrl = await task;

                return downloadUrl;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }
    }
}
