using MySql.Data.MySqlClient;
using MtgApiManager.Lib.Model;
using MtgApiManager.Lib.Service;
using MtgApiManager.Lib.Core;
using MtgApiManager.Lib.Dto;
using System;
using System.Collections.Generic;

namespace Magic.Models
{
    public class DBCard
    {
        private int _id;
        public int Id {get{return _id;}}
        
        private string _api_id;
        public string ApiId {get{return _api_id;}}

        private string _name;
        public string Name { get{return _name;} }

        private string _manaCost;
        public string ManaCost { get{return _manaCost;} }

        private string _colors;
        public string Colors { get{return _colors;} }

        private string _type;
        public string Type { get{return _type;} }

        private string _toughness;
        public string Toughness { get{return _toughness;} }

        private string _power;
        public string Power { get{return _power;} }

        private string _imgUrl;
        public string ImgUrl { get{return _imgUrl;} }

        private string _text;
        public string Text { get{return _text;} }

        public DBCard(int id, string api_id, string name, string manaCost, string colors, string type, string toughness, string power, string imgUrl, string text)
        {
            _id = id;
            _api_id = api_id;
            _name = name;
            _manaCost = manaCost;
            _colors = colors;
            _type = type;
            _toughness = toughness;
            _power = power;
            _imgUrl = imgUrl;
            _text = text;
        }

        public static bool InDB(string searchId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT COUNT(*) FROM cards WHERE api_id = @searchId;";
            MySqlParameter _searchId = new MySqlParameter("@searchId", searchId);
            cmd.Parameters.Add(_searchId);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                if(rdr.GetInt32(0) > 0)
                {
                    conn.Close();
                    if(conn!=null){conn.Dispose();}
                    return true;
                } 
            }

            conn.Close();
            if(conn!=null){conn.Dispose();}
            
            return false;
            
        }

        public static void SaveCard(string cardId)
        {
            CardService service = new CardService();
            var result = service.Find(cardId);

            if(!InDB(result.Value.Id))
            {
                MySqlConnection conn = DB.Connection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
                cmd.CommandText = @"INSERT INTO cards (api_id, name, mana_cost, colors, type, toughness, power, image_url, text) 
                    VALUES (@api_id, @name, @mana_cost, @colors, @type, @toughness, @power, @image_url, @text);";

                MySqlParameter api_id = new MySqlParameter("@api_id", result.Value.Id);
                cmd.Parameters.Add(api_id);
                MySqlParameter name = new MySqlParameter("@name", result.Value.Name);
                cmd.Parameters.Add(name);
                MySqlParameter mana_cost = new MySqlParameter("@mana_cost", result.Value.ManaCost);
                cmd.Parameters.Add(mana_cost);
                MySqlParameter colors = new MySqlParameter("@colors", result.Value.Colors);
                cmd.Parameters.Add(colors);
                MySqlParameter type = new MySqlParameter("@type", result.Value.Type);
                cmd.Parameters.Add(type);
                MySqlParameter toughness = new MySqlParameter("@toughness", result.Value.Toughness);
                cmd.Parameters.Add(toughness);
                MySqlParameter power = new MySqlParameter("@power", result.Value.Power);
                cmd.Parameters.Add(power);
                MySqlParameter image_url = new MySqlParameter("@image_url", result.Value.ImageUrl.ToString());
                cmd.Parameters.Add(image_url);
                MySqlParameter text = new MySqlParameter("@text", result.Value.Text);
                cmd.Parameters.Add(text);

                cmd.ExecuteNonQuery();
                
                conn.Close();
                if(conn != null){conn.Dispose();}
            }
        }

        public static List<DBCard> GetAll()
        {
            List<DBCard> allCards = new List<DBCard>{};

            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM cards;";
            MySqlDataReader rdr = cmd.ExecuteReader();


            string mana, colors, type, toughness, power, imgUrl, text;
            mana = colors = type = toughness = power = imgUrl = text = "n/a";

            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string apiId = rdr.GetString(1);
                string name = rdr.GetString(2);

                if(!rdr.IsDBNull(3)) { mana = rdr.GetString(3); }
                if(!rdr.IsDBNull(4)) { colors = rdr.GetString(4); }
                if(!rdr.IsDBNull(5)) { type = rdr.GetString(5); }
                if(!rdr.IsDBNull(6)) { toughness = rdr.GetString(6); }
                if(!rdr.IsDBNull(7)) { power = rdr.GetString(7); }
                if(!rdr.IsDBNull(8)) { imgUrl = rdr.GetString(8); }
                if(!rdr.IsDBNull(9)) { text = rdr.GetString(9); }

                DBCard card = new DBCard(id, apiId, name, mana, colors, type, toughness, power, imgUrl, text);
                allCards.Add(card);
            }
            
            conn.Close();
            if(conn!=null) conn.Dispose();

            return allCards;
        }
    }
}
