using System.ComponentModel.DataAnnotations;
using System.Data;

namespace OSProject.DAL
{
    public class OSRating
    {
        public int game_id { get; set; }
        public int user_id { get; set; }
        public int game_score { get; set; }

        public OSRating(int Game_Id, int User_Id, int Game_Score)
        {
            game_id = Game_Id;
            user_id = User_Id;
            game_score = Game_Score;
        }
    }
}
