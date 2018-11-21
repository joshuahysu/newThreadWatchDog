using System;
using System.Threading;
using System.Data.SqlClient;

namespace newThreadWatchDog
{
    public class ThreadWatchDog
    {
        public volatile bool boolIsRun=true;//break Thread while loop
        public static string connectionString;//db connection String
        private ThreadWatchDog() {
        }
        public ThreadWatchDog(Action newThreadFunc,string sql)//sql=$"insert WatchDogTable('ID','threadName','datetime') values('{Guid.new()}','{}','{datetime.now}')"
        {
            try
            {
                new Thread(() => {
                    while (boolIsRun)
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            using (SqlCommand cmd = new SqlCommand(sql, connection))
                            {
                                cmd.ExecuteNonQuery();
                            }
                        }
                        newThreadFunc();
                    }
                }).Start();
            }
            catch { throw; }
        }
    }
}
