using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SQLite;

namespace balance.DataBase
{
    /// <summary>
    /// データベース管理クラス
    /// </summary>
    public static class DBConnect
    {
        
        ////////////////////////////////////////////////////////////////////////////////
        ///メンバ
        ///////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// データベースファイルの場所
        /// </summary>
        private static string DatabaseFilePath;

        /// <summary>
        /// データベースコネクション
        /// </summary>
        private static SQLiteConnection conn;

        /// <summary>
        /// リーダー（結果の読み取り）
        /// </summary>
        public static SQLiteDataReader Reader;

        ////////////////////////////////////////////////////////////////////////////////
        ///メソッド
        ///////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// データベースに接続します
        /// </summary>
        /// <param name="databaseFilePath">接続先データベースファイルの場所</param>
        public static void Connect(string databaseFilePath)
        {
            //データベースに接続する
            conn = new SQLiteConnection("Data Source=" + databaseFilePath);
            DBConnect.DatabaseFilePath = databaseFilePath;

            //データベースを開く
            conn.Open();
        }


        /// <summary>
        /// 戻り値のないSQL命令を実行します
        /// </summary>
        /// <param name="sql">実行するSQL命令</param>
        public static void ExecuteNonQuery(String sql)
        {
            //データベースに命令を与えるための用意をする
            SQLiteCommand cmd = conn.CreateCommand();

            //SQL命令の発行
            cmd.CommandText = sql;

            //SQL命令の実行（取得するデータがない場合はNonQueryですね）
            cmd.ExecuteNonQuery();

            //リソースの解放
            cmd.Dispose();
            cmd = null;
        }

        /// <summary>
        /// 戻り値のない複数のSQL命令を実行します
        /// </summary>
        /// <param name="sql">実行するSQL命令</param>
        public static void ExecuteNonQuery(List<String> sqls)
        {
            SQLiteTransaction sqlt = conn.BeginTransaction();
            SQLiteCommand cmd = conn.CreateCommand();

            foreach (string sql in sqls)
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }

            sqlt.Commit();

            //リソースの解放
            cmd.Dispose();
            cmd = null;
        }


        /// <summary>
        /// 戻り値にあるSQL命令を事項します
        /// </summary>
        /// <param name="sql">実行するSQL命令</param>
        public static void ExecuteReader(string sql)
        {
            //データベースに命令を与えるための用意をする
            SQLiteCommand cmd = conn.CreateCommand();

            //SQL命令の発行
            cmd.CommandText = sql;

            //もし以前に読み込んだ結果が閉じられていなかったときはあらかじめ閉じておきます
            if (Reader != null)
            {
                if (Reader.IsClosed == false) Reader.Close();
                Reader = null;

            }

            //SQL命令の実行（取得するデータがない場合はNonQueryですね）
            Reader = cmd.ExecuteReader();

            //リソースの解放
            cmd.Dispose();
            cmd = null;
        }



        /// <summary>
        /// データベースへの接続を閉じ、リソースを開放します
        /// </summary>
        public static void Dispose()
        {
            //もし以前に読み込んだ結果が閉じられていなかったときはあらかじめ閉じておきます
            if (Reader != null)
            {
                if (Reader.IsClosed == false) Reader.Close();
                Reader = null;

            }

            //データベースの接続を閉じる
            conn.Close();

            //リソースの開放
            conn.Dispose();
            conn = null;
        }

      
    }
}
