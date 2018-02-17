using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System.Linq;
using Plugin.Media.Abstractions;

namespace FaceAPIFunctions
{
    public class Face0
    {
        private readonly IFaceServiceClient faceServiceClient = new FaceServiceClient("eda91d84d8d74e99b5afa36073cea990", "https://southeastasia.api.cognitive.microsoft.com/face/v1.0");
        String groupId = "001";
       

        public Face0()
        {

        }

        
     
        
        public async Task<String> register(MediaFile file,int Id)
        {
            Stream path = file.GetStream();
            string UserId = Id.ToString();
            var faces= await detectFace(path,UserId);
            var faceIds = faces.Select(face => face.FaceId).ToArray();
            if (faces.Length == 0)
            {
                return ("No faces Detected");
            }
            if (faces.Length > 1)
            {
                return ("More than one face detected");
            }
            else
            {
                Stream path2 = file.GetStream();
                var result = await addperson(path2, UserId);
                return result;
            }
            

        }
        private async Task<Face[]> detectFace(Stream path, String personId)
        {

            using (path)
            {

                var faces = await faceServiceClient.DetectAsync(path);
                return faces;
            }                  

        }
        public async Task<String> detectFaceTrue(MediaFile path)
        {

            using (Stream s = path.GetStream())
            {

                var faces = await faceServiceClient.DetectAsync(s);
                if (faces.Length == 0)
                {
                    return "no face detected";
                }
                if (faces.Length > 1)
                {
                    return "more than one face detected";
                }
                else
                {
                    return "face detected successfully";
                }
            }

        }
        private async Task<String> addperson(Stream path, String personId)
        {
            try
            {
                CreatePersonResult persons = await faceServiceClient.CreatePersonAsync(groupId, personId);

                var personIds = persons.PersonId;
                using (path)
                {
                    var persistedperson = await faceServiceClient.AddPersonFaceAsync(groupId, personIds, path);
                    await faceServiceClient.TrainPersonGroupAsync(groupId);
                    return "Success";
                }
            }
            catch ( Exception e)
            {
                return e.Message;
                
            }

        }
         
       
        public async Task<int> search(MediaFile file)
        {
            
            using (Stream s = file.GetStream())
            {

                int i;
                var faces = await faceServiceClient.DetectAsync(s);
                if (faces.Length == 0)
                {
                    i = -1;
                    return i;
                }
                else
                {
                    var faceIds = faces.Select(face => face.FaceId).ToArray();

                    var results = await faceServiceClient.IdentifyAsync(groupId, faceIds);
                    foreach (var identifyResult in results)
                    {
                        
                        if (identifyResult.Candidates.Length == 0)
                        {
                            return 0;
                        }
                        else
                        {
                            // Get top 1 among all candidates returned
                            var candidateId = identifyResult.Candidates[0].PersonId;
                            var person = await faceServiceClient.GetPersonAsync(groupId, candidateId);
                            try
                            {
                                return Convert.ToInt32(person.Name);
                                
                            }
                            catch (Exception)
                            {
                                return 0;
                            }

                        }
                        
                    }
                      
                }
                return 0;
            }
        }

        public async Task<int> searchFirst(MediaFile file)
        {

            using (Stream path = file.GetStream())
            {
                var faces = await faceServiceClient.DetectAsync(path);
                if (faces.Length == 0) { return 0; }//no one detected
                if (faces.Length > 1) { return 1; }//more than one person detected
                else
                {
                    var faceIds = faces.Select(face => face.FaceId).ToArray();

                    var results = await faceServiceClient.IdentifyAsync(groupId, faceIds);
                    foreach (var identifyResult in results)
                    {

                        if (identifyResult.Candidates.Length == 0)
                        {
                            return 3;// success
                        }
                        else
                        {
                            return 2;//there is an existing person
                        }
                    }
                    return 0;

                }
            }
        }

    }
            
 
}
