using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace RifleRange.DAL
{
    public class rrImage
    {
        [SqlField]
        public int PhotoAlbumId { get; set; }
        [SqlField]
        public int ImageId { get; set; }
        [SqlField]
        public string Description { get; set; }
        [SqlField]
        public string FileName { get; set; }
        [SqlField]
        public DateTime CreateDate { get; set; }

        static rrImage()
        {
            RecordMapper.Register(typeof(rrImage), typeof(SqlField));
        }
        public rrImage(SqlDataReader reader)
        {
            RecordMapper.ReadObject(this, reader);
        }
    }

    public class rrImageDB
    {
        public static rrImage GetImage(int ImageId)
        {
            rrImage result = null;
            using (SqlConnection conn = new SqlConnection(SQL.RifleRange))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("dbo.usp_GetImage", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ImageId", ImageId);
                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null && !reader.IsClosed)
                    {
                        while (reader.Read()) result = new rrImage(reader);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }
            return result;
        }

        public static LinkedList<rrImage> GetImage(int? PhotoAlbumId = null)
        {
            LinkedList<rrImage> result = new LinkedList<rrImage>();
            using (SqlConnection conn = new SqlConnection(SQL.RifleRange))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("dbo.usp_GetImage", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (PhotoAlbumId != null) cmd.Parameters.AddWithValue("@PhotoAlbumId", PhotoAlbumId);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null && !reader.IsClosed)
                    {
                        result = new LinkedList<rrImage>();
                        while (reader.Read())
                        {
                            rrImage obj = new rrImage(reader);
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

        public static void DeleteImage(int ImageId)
        {
            using (SqlConnection conn = new SqlConnection(SQL.RifleRange))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("dbo.usp_DeleteImage", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ImageId", ImageId);
                    conn.Open();
                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }
        }

        public static void UpdateImage(int ImageId, string Description, int PhotoAlbumId)
        {
            using (SqlConnection conn = new SqlConnection(SQL.RifleRange))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("dbo.usp_UpdateImage", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ImageId", ImageId);
                    cmd.Parameters.AddWithValue("@Description", Description);
                    cmd.Parameters.AddWithValue("@PhotoAlbumId", PhotoAlbumId);
                    conn.Open();
                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }
        }

        public static int InsertImage(string FileName, string Description, int PhotoAlbumId)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(SQL.RifleRange))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("dbo.usp_InsertImage", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FileName", FileName);
                    cmd.Parameters.AddWithValue("@Description", Description);
                    cmd.Parameters.AddWithValue("@PhotoAlbumId", PhotoAlbumId);
                    SqlParameter NewImageId = new SqlParameter("@NewImageId", SqlDbType.Int, 10);
                    NewImageId.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(NewImageId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    result = (int)NewImageId.Value;

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
