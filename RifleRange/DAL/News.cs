﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

//--------------------------------------------------------------------------------
//-- Generated By: Slava
//-- Date Generated: 12/11/2016
//--------------------------------------------------------------------------------
namespace RifleRange.DAL
{

    public class rrNews
    {
        [SqlField]
        public int NewsId { get; set; }
        [SqlField]
        public string Title { get; set; }
        [SqlField]
        public string Body { get; set; }
        [SqlField]
        public string FileName { get; set; }
        [SqlField]
        public DateTime CreateDate { get; set; }
        [SqlField]
        public DateTime? LastUpdate { get; set; }

        static rrNews()
        {
            RecordMapper.Register(typeof(rrNews), typeof(SqlField));
        }
        public rrNews(SqlDataReader reader)
        {
            RecordMapper.ReadObject(this, reader);
        }
        public rrNews()
        {

        }
    }

    public class rrNewsDB
    {
        public static int RowCnt;
        public static LinkedList<rrNews> GetNewsList()
        {
            LinkedList<rrNews> result = new LinkedList<rrNews>();
            using (SqlConnection conn = new SqlConnection(SQL.RifleRange))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("dbo.usp_GetNews", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null && !reader.IsClosed)
                    {
                        result = new LinkedList<rrNews>();
                        while (reader.Read())
                        {
                            rrNews obj = new rrNews(reader);
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
        public static rrNews GetNews(int NewsId)
        {
            rrNews result = null;
            using (SqlConnection conn = new SqlConnection(SQL.RifleRange))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("dbo.usp_GetNews", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NewsId", NewsId);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null && !reader.IsClosed)
                    {
                        while (reader.Read())
                        {
                            result = new rrNews(reader);
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
        public static void DeleteNews(int NewsId)
        {
            using (SqlConnection conn = new SqlConnection(SQL.RifleRange))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("dbo.usp_DeleteNews", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NewsId", NewsId);
                    conn.Open();
                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }
        }

        public static void UpdateNews(int NewsId, string Title, string Body, string FileName = null)
        {
            using (SqlConnection conn = new SqlConnection(SQL.RifleRange))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("dbo.usp_UpdateNews", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NewsId", NewsId);
                    cmd.Parameters.AddWithValue("@Title", Title);
                    cmd.Parameters.AddWithValue("@Body", Body);
                    if (FileName != null) cmd.Parameters.AddWithValue("@FileName", FileName);
                    conn.Open();
                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }
        }

        public static int InsertNews(string Title, string Body, string FileName = null)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(SQL.RifleRange))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("dbo.usp_InsertNews", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Title", Title);
                    cmd.Parameters.AddWithValue("@Body", Body);
                    SqlParameter NewNewsId = new SqlParameter("@NewNewsId", SqlDbType.Int, 4);
                    NewNewsId.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(NewNewsId);
                    if (FileName != null) cmd.Parameters.AddWithValue("@FileName", FileName);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    result = (int)NewNewsId.Value;

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
