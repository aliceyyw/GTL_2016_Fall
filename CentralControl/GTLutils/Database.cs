using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Configuration;
using System.Collections;
using System.Text;
using CentralControl;

namespace GTLutils
{

    public class Database
    {
       
        string error;
        List<List<int>> errorid;

       
        private static string sqlCreator(string tablename ,string[] args)
        {
            StringBuilder str =new StringBuilder("insert into ");
            str.Append(tablename+" values(");
            foreach (string s in args)
            {
                if (s.Equals("NULL"))
                    str.Append(s + ",");
                else
                    str.Append("'" + s + "'" + ",");
            }
            int len = str.Length;
            str.Remove(len - 1, 1);

            str.Append(")");
            return str.ToString();
         }
  

        public static string insertTable(string tablename, ArrayList list)
        {
            string[] args = (string[])list.ToArray(typeof(string));
            string sql = sqlCreator(tablename,args);
            DBUtil.executedNonQueryCmd(sql);
             return sql;
        }

       

       

       

        //public bool inserthacod(int device_id, /*List<List<int>>*/int[][] value, int device_state)
        //{
        //    int state;
        //    errorid = new List<List<int>>();
        //    bool fine = true;
        //    for (int i = 0; i < 8; i++)
        //    {
        //        List<int> line = new List<int>();
        //        for (int j = 0; j < 12; j++)
        //        {
        //            state = insert("insert into HAC_OD values(" + device_state.ToString() + "," + j.ToString() + "," + i.ToString() + "," + value[i][j].ToString() + ",'" + DateTime.Now.ToString() + "'," + device_id + ")");
        //            line.Add(state);
        //            if (state != -1)
        //            {
        //                fine = false;
        //            }
        //        }
        //        errorid.Add(line);
        //    }
        //    return fine;
        //}

        
        
        
     

    }
}
