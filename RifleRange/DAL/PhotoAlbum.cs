using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace RifleRange.DAL
{
    public enum StandardPhotoAlbum
    {
        Restaurant = 3,
        Dock = 4,
        Hourse = 6,
        Skeet = 7,
        Sporting = 8,
        RifleRange = 9
    }
    public class rrPhotoAlbum
    {
        [SqlField]
        public int AlbumId { get; set; }
        [SqlField]
        public string Title { get; set; }
        [SqlField]
        public string Description { get; set; }
        [SqlField]
        public DateTime CreateDate { get; set; }
        [SqlField]
        public DateTime? LastUpdate { get; set; }

        static rrPhotoAlbum()
        {
            RecordMapper.Register(typeof(rrPhotoAlbum), typeof(SqlField));
        }
        public rrPhotoAlbum(SqlDataReader reader)
        {
            RecordMapper.ReadObject(this, reader);
        }
    }

    public class rrPhotoAlbumDB
    {
        public static rrPhotoAlbum GetPhotoAlbum(int PhotoAlbumId)
        {
            rrPhotoAlbum result = null;
            using (SqlConnection conn = new SqlConnection(SQL.RifleRange))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("dbo.usp_GetPhotoAlbum", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PhotoAlbumId", PhotoAlbumId);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null && !reader.IsClosed)
                    {
                        while (reader.Read())
                        {
                            result = new rrPhotoAlbum(reader);
                        }
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }
            return result;
        }
        public static LinkedList<rrPhotoAlbum> GetPhotoAlbum()
        {
            LinkedList<rrPhotoAlbum> result = new LinkedList<rrPhotoAlbum>();
            using (SqlConnection conn = new SqlConnection(SQL.RifleRange))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("dbo.usp_GetPhotoAlbum", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null && !reader.IsClosed)
                    {
                        result = new LinkedList<rrPhotoAlbum>();
                        while (reader.Read())
                        {
                            rrPhotoAlbum obj = new rrPhotoAlbum(reader);
                            result.AddLast(obj);
                        }
                        reader.Close();
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }
            return result;
        }

    }
}
