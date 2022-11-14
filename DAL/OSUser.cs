using System.ComponentModel.DataAnnotations;
using System.Data;

namespace OSProject.DAL
{
    public class OSUser
    {
        [Key]
        public int id { get; set; }
        public string user_name { get; set; }
        public string stype { get; set; }
        public string user_password { get; set; }
        public string image { get; set; }
        public int user_score { get; set; }

        public OSUser (int Id, string User_Name, string Stype, string User_Password, string Image, int User_Score) { 
            id = Id;
            user_name = User_Name;
            stype = Stype;
            user_password = User_Password;
            image = Image;
            user_score = User_Score;
        }
    }
}
