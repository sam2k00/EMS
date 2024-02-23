using EMS.WebMvc.Models;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;

using Microsoft.Data.Sqlite;

namespace EMS.WebMvc.Helper
{
    public class ElactionCommissionHelper
    {
        
		private readonly IConfiguration _configuration;
		private readonly SqlConnection _cnn;

        private readonly SqliteConnection _connection;
        public ElactionCommissionHelper(IConfiguration configuration)
		{
			_configuration = configuration;
            _cnn = new SqlConnection(_configuration["ConnectionStrings:EmsApiConnection"].ToString());

            _connection = new SqliteConnection(_configuration["ConnectionStrings:EmsSqliteConnection"].ToString());

        }

        public List<Candidate> Loadcandidate()
        {
            var candidates = new List<Candidate>();

            string strCmd = @"select c.CandidateId,c.Name,c.StateId,c.PartyId,p.PartyName,s.StateName from Candidates c join Parties as p on c.PartyId=p.PartyId join States s on c.StateId=s.StateId";

            var command = new SqliteCommand(strCmd, _connection);
            _connection.Open();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var candidate = new Candidate()
                {
                    CandidateId = Convert.ToInt32(reader["CandidateId"]),
                    Name = reader["Name"].ToString(),
                    PartyId = Convert.ToInt32(reader["PartyId"]),
                    StateId = Convert.ToInt32(reader["StateId"]),
                    PartyName = reader["PartyName"].ToString(),
                    StateName = reader["StateName"].ToString()
                };
                candidates.Add(candidate);
            }
            _connection.Close();

            return candidates;
        }

        public List<Candidate> LoadStateCandidates(int stateID)
        {
            var candidates = new List<Candidate>();

            string strCmd = @"select c.CandidateId,c.Name,c.StateId,c.PartyId,p.PartyName,s.StateName 
                             from Candidates c 
                             join Parties p on c.PartyId=p.PartyId 
                             join States s on c.StateId=s.StateId 
                             where s.StateId=@StateId";

            using (var command = new SqliteCommand(strCmd, _connection))
            {
                command.Parameters.AddWithValue("@StateId", stateID);
                _connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var candidate = new Candidate()
                        {
                            CandidateId = reader.GetInt32(reader.GetOrdinal("CandidateId")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            PartyId = reader.GetInt32(reader.GetOrdinal("PartyId")),
                            StateId = reader.GetInt32(reader.GetOrdinal("StateId")),
                            PartyName = reader.GetString(reader.GetOrdinal("PartyName")),
                            StateName = reader.GetString(reader.GetOrdinal("StateName"))
                        };
                        candidates.Add(candidate);
                    }
                }
            }

            return candidates;
        }

        public List<States> Loadstates()
        {
            var states = new List<States>();

            string strCmd = @"select * from States";

            using (var command = new SqliteCommand(strCmd, _connection))
            {
                _connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var state = new States()
                        {
                            StateId = reader.GetInt32(reader.GetOrdinal("StateId")),
                            StateName = reader.GetString(reader.GetOrdinal("StateName")),
                            MpSeats = reader.GetInt32(reader.GetOrdinal("MpSeats"))
                        };
                        states.Add(state);
                    }
                }
            }

            return states;
        }

        public List<Partie> Loadparties()
        {
            var parties = new List<Partie>();

            string strCmd = @"select P.PartyId,P.PartyName,P.SymbolId,Sy.SymbolName 
                             from Parties P 
                             join Symbols Sy on P.SymbolId=Sy.SymbolId";

            using (var command = new SqliteCommand(strCmd, _connection))
            {
                _connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var partie = new Partie()
                        {
                            PartyId = reader.GetInt32(reader.GetOrdinal("PartyId")),
                            PartyName = reader.GetString(reader.GetOrdinal("PartyName")),
                            SymbolId = reader.GetInt32(reader.GetOrdinal("SymbolId")),
                            PartiSymbol = new Symbol()
                            {
                                SymbolId = reader.GetInt32(reader.GetOrdinal("SymbolId")),
                                SymbolName = reader.GetString(reader.GetOrdinal("SymbolName"))
                            }
                        };
                        parties.Add(partie);
                    }
                }
            }

            return parties;
        }


        /*
         * 
         * 
        
        public List<Candidate> Loadcandidate()
        {
            var candidates = new List<Candidate>();

            string str = _cnn.ConnectionString;
                        
            string strCmd = @"select c.CandidateId,c.Name,c.StateId,c.PartyId,p.PartyName,s.StateName from [dbo].[Candidates] c join Parties as p on c.PartyId=p.PartyId join States s on c.StateId=s.StateId";

            SqlDataAdapter da = new SqlDataAdapter(strCmd, _cnn);
            da.SelectCommand.CommandType = CommandType.Text;

            DataSet DS = new DataSet();
            da.Fill(DS);
            DataTable dt = DS.Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var candidate = new Candidate()
                {
                    CandidateId = Convert.ToInt32(dt.Rows[i]["CandidateId"]),
                    Name = dt.Rows[i]["Name"].ToString(),
                    PartyId= Convert.ToInt32(dt.Rows[i]["PartyId"]),
                    StateId= Convert.ToInt32(dt.Rows[i]["StateId"]),
                    PartyName = dt.Rows[i]["PartyName"].ToString(),
                    StateName= dt.Rows[i]["StateName"].ToString()
                };
                candidates.Add(candidate);
            } 

			return candidates;
        }

        public List<Candidate> LoadStateCandidates(int stateID)
        {
            var candidates = new List<Candidate>();

            string str = _cnn.ConnectionString;

            string strCmd = @"select c.CandidateId,c.Name,c.StateId,c.PartyId,p.PartyName,s.StateName from [dbo].[Candidates] c join Parties as p on c.PartyId=p.PartyId join States s on c.StateId=s.StateId where s.StateId="+stateID+"";

            SqlDataAdapter da = new SqlDataAdapter(strCmd, _cnn);
            da.SelectCommand.CommandType = CommandType.Text;

            DataSet DS = new DataSet();
            da.Fill(DS);
            DataTable dt = DS.Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var candidate = new Candidate()
                {
                    CandidateId = Convert.ToInt32(dt.Rows[i]["CandidateId"]),
                    Name = dt.Rows[i]["Name"].ToString(),
                    PartyId = Convert.ToInt32(dt.Rows[i]["PartyId"]),
                    StateId = Convert.ToInt32(dt.Rows[i]["StateId"]),
                    PartyName = dt.Rows[i]["PartyName"].ToString(),
                    StateName = dt.Rows[i]["StateName"].ToString()
                };
                candidates.Add(candidate);
            }

            return candidates;
        }

        public List<States> Loadstates() {
            var states = new List<States>();
			//SqlDataAdapter da = new SqlDataAdapter("sp_LoadStates", cnn);
			//da.SelectCommand.CommandType = CommandType.StoredProcedure;

			SqlDataAdapter da = new SqlDataAdapter("select * from [dbo].[States]", _cnn);
			da.SelectCommand.CommandType = CommandType.Text;

			DataSet DS = new DataSet();
            da.Fill(DS);
            DataTable dt = DS.Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var _state = new States()
                {
					StateId = Convert.ToInt32(dt.Rows[i]["StateId"]),
					StateName = dt.Rows[i]["StateName"].ToString(),
                    MpSeats= Convert.ToInt32(dt.Rows[i]["MpSeats"])
				};
                states.Add(_state);
            }

            return states;
        }

        public List<Partie> Loadparties()
        {
            var parties = new List<Partie>();
            //SqlDataAdapter da = new SqlDataAdapter("sp_LoadStates", cnn);
            //da.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter("select P.PartyId,P.PartyName,P.SymbolId,Sy.SymbolName from [dbo].[Parties] as P join [dbo].[Symbols] as Sy on P.SymbolId=Sy.SymbolId", _cnn);
            da.SelectCommand.CommandType = CommandType.Text;

            DataSet DS = new DataSet();
            da.Fill(DS);
            DataTable dt = DS.Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var _partie = new Partie()
                {
                    PartyId = Convert.ToInt32(dt.Rows[i]["PartyId"]),
                    PartyName = dt.Rows[i]["PartyName"].ToString(),
                    SymbolId = Convert.ToInt32(dt.Rows[i]["SymbolId"]),
					PartiSymbol = new Symbol() { 
                        SymbolId= Convert.ToInt32(dt.Rows[i]["SymbolId"]),                        
                        SymbolName= dt.Rows[i]["SymbolName"].ToString()
					}
				};
                parties.Add(_partie);
            }

            return parties;
        }
        
        public Partie LoadpartyDetails(int id)
        {
            var partie = new Partie();
            //SqlDataAdapter da = new SqlDataAdapter("sp_LoadStates", cnn);
            //da.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter("select P.PartyId,P.PartyName,P.SymbolId,Sy.SymbolName from [dbo].[Parties] as P join [dbo].[Symbols] as Sy on P.SymbolId=Sy.SymbolId where p.PartyId=" + id + "", _cnn);
            da.SelectCommand.CommandType = CommandType.Text;

            DataSet DS = new DataSet();
            da.Fill(DS);
            DataTable dt = DS.Tables[0];

            if (dt.Rows.Count > 0)
            {
                partie.PartyId = Convert.ToInt32(dt.Rows[0]["PartyId"]);
                partie.PartyName = dt.Rows[0]["PartyName"].ToString();
                partie.SymbolId = Convert.ToInt32(dt.Rows[0]["SymbolId"]);
                partie.PartiSymbol = new Symbol()
                {
                    SymbolId = Convert.ToInt32(dt.Rows[0]["SymbolId"]),
                    SymbolName = dt.Rows[0]["SymbolName"].ToString()
                };
            }       



            return partie;
        }

        public List<Voter> Loadvoters()
        {
            var voters = new List<Voter>();
            //SqlDataAdapter da = new SqlDataAdapter("sp_LoadStates", cnn);
            //da.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter("select * from [dbo].[Voters]", _cnn);
            da.SelectCommand.CommandType = CommandType.Text;

            DataSet DS = new DataSet();
            da.Fill(DS);
            DataTable dt = DS.Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var _voter = new Voter()
                {
                    VoterId = Convert.ToInt32(dt.Rows[i]["VoterId"]),
                    Name = dt.Rows[i]["Name"].ToString(),
                    Address = dt.Rows[i]["Address"].ToString(),
                    Photo = dt.Rows[i]["Photo"].ToString(),
                    Status = Convert.ToBoolean(dt.Rows[i]["Status"]),                    
                    StateId = Convert.ToInt32(dt.Rows[i]["StateId"])
                };
                voters.Add(_voter);
            }

            return voters;
        }

        public void SaveVoter(Voter voter)
        {
            if (voter.VoterId == 0)
            {

                string strCmd = @"insert into Voters values('"+ voter.Name + "','"+voter.Address+"','"+voter.Photo+"','"+voter.Status+"',"+voter.StateId+",'JH234')";
                SqlCommand cmd = new SqlCommand(strCmd, _cnn);
                cmd.CommandType = CommandType.Text;

                _cnn.Open();
                cmd.ExecuteNonQuery();
                _cnn.Close();
            }
            else
            {
                string strCmd = @"update Voters set Name='"+voter.Name+"',Address='"+voter.Address+"', Photo='"+voter.Photo+"', Status='"+voter.Status+"', StateId="+voter.StateId+" where VoterId="+voter.VoterId+"";

                SqlCommand cmd = new SqlCommand(strCmd, _cnn);
                cmd.CommandType = CommandType.Text;

                _cnn.Open();
                cmd.ExecuteNonQuery();
                _cnn.Close();
            }
        }

        public Voter GetVoter(int voterId)
        {
            var voter = new Voter();

            string strCmd = @"select * from Voters where VoterId="+voterId+"";

            SqlDataAdapter da = new SqlDataAdapter(strCmd, _cnn);
            da.SelectCommand.CommandType = CommandType.Text;

            DataSet DS = new DataSet();
            da.Fill(DS);
            DataTable dt = DS.Tables[0];

            if (dt.Rows.Count > 0)
            {
                voter.VoterId = Convert.ToInt32(dt.Rows[0]["VoterId"]);
                voter.Name = dt.Rows[0]["Name"].ToString();
                voter.Address= dt.Rows[0]["Address"].ToString();
                voter.Photo= dt.Rows[0]["Photo"].ToString();
                voter.Status= Convert.ToBoolean(dt.Rows[0]["Status"]);
                voter.StateId= Convert.ToInt32(dt.Rows[0]["StateId"]);
            }

            return voter;
        }

        public Candidate Loadcandidatedetails(int _candidateId)
        {
            var candidate = new Candidate();

            string str = _cnn.ConnectionString;

            //SqlDataAdapter da = new SqlDataAdapter("sp_LoadCandidates", cnn);
            //da.SelectCommand.CommandType = CommandType.StoredProcedure;

            string strCmd = @"select c.CandidateId,c.Name,c.StateId,c.PartyId,p.PartyName,s.StateName from [dbo].[Candidates] c join Parties as p on c.PartyId=p.PartyId join States s on c.StateId=s.StateId where c.CandidateId="+ _candidateId;

            SqlDataAdapter da = new SqlDataAdapter(strCmd, _cnn);
            da.SelectCommand.CommandType = CommandType.Text;

            DataSet DS = new DataSet();
            da.Fill(DS);
            DataTable dt = DS.Tables[0];

            if(dt.Rows.Count > 0)
            {
                candidate.CandidateId = Convert.ToInt32(dt.Rows[0]["CandidateId"]);
                candidate.Name = dt.Rows[0]["Name"].ToString();
                candidate.PartyId = Convert.ToInt32(dt.Rows[0]["PartyId"]);
                candidate.StateId = Convert.ToInt32(dt.Rows[0]["StateId"]);
                candidate.PartyName = dt.Rows[0]["PartyName"].ToString();
                candidate.StateName = dt.Rows[0]["StateName"].ToString();
            }

            return candidate;
        }

        public void SaveCandidate(Candidate candidate)
        {
            if (candidate.CandidateId == 0)
            {
                //insert into Candidates values('sa2',2,2)
                SqlCommand cmd = new SqlCommand(@"insert into Candidates values('" + candidate.Name + "'," + candidate.PartyId + "," + candidate.StateId + ")", _cnn);
                cmd.CommandType = CommandType.Text;

                _cnn.Open();
                cmd.ExecuteNonQuery();
                _cnn.Close();

            }
            else
            {

                string strCmd = @"update [dbo].[Candidates] SET [Name] = '" + candidate.Name + "', [PartyId] = " + candidate.PartyId + ", [StateId]=" + candidate.StateId + " WHERE [CandidateID] = " + candidate.CandidateId + ";";

                SqlCommand cmd = new SqlCommand(strCmd, _cnn);
                cmd.CommandType = CommandType.Text;

                _cnn.Open();
                cmd.ExecuteNonQuery();
                _cnn.Close();
            }
        }

        public List<Symbol> GetSymbols() {

            var symbols = new List<Symbol>();
            
            SqlDataAdapter da = new SqlDataAdapter("select * from [dbo].[Symbols]", _cnn);
            da.SelectCommand.CommandType = CommandType.Text;

            DataSet DS = new DataSet();
            da.Fill(DS);
            DataTable dt = DS.Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var _symbol = new Symbol()
                {
                    SymbolId = Convert.ToInt32(dt.Rows[i]["SymbolId"]),
                    SymbolName = dt.Rows[i]["SymbolName"].ToString()                    
                };
                symbols.Add(_symbol);
            }

            return symbols;

        }

        public void SaveParty(Partie partie)
        {
            if (partie.PartyId == 0) 
            {
                
                string strCmd = "insert into [dbo].[Parties] values('"+ partie.PartyName+ "',"+partie.SymbolId+")";
                SqlCommand cmd = new SqlCommand(strCmd, _cnn);
                cmd.CommandType = CommandType.Text;

                _cnn.Open();
                cmd.ExecuteNonQuery();
                _cnn.Close();
            }
            else
            {

                string strCmd = @"update Parties set PartyName='" + partie.PartyName + "',SymbolId=" + partie.SymbolId + " where PartyId=" + partie.PartyId + "";

                //string strCmd = "insert into [dbo].[Parties] values('" + partie.PartyName + "'," + partie.SymbolId + ")";
                SqlCommand cmd = new SqlCommand(strCmd, _cnn);
                cmd.CommandType = CommandType.Text;

                _cnn.Open();
                cmd.ExecuteNonQuery();
                _cnn.Close();

            }

        }

        public List<States> GetStates()
        {
            var states = new List<States>();
            
            SqlDataAdapter da = new SqlDataAdapter("select * from States", _cnn);
            da.SelectCommand.CommandType = CommandType.Text;

            DataSet DS = new DataSet();
            da.Fill(DS);
            DataTable dt = DS.Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var _states = new States()
                {
                    StateId = Convert.ToInt32(dt.Rows[i]["StateId"]),
                    StateName = dt.Rows[i]["StateName"].ToString(),
                    MpSeats = Convert.ToInt32(dt.Rows[i]["MpSeats"]),
                    
                };
                states.Add(_states);
            }

            return states;

        }

        public void SaveState(States state)
        {
            if (state.StateId == 0)
            {
                //insert new state
                string strCmd = @"insert into States values ('"+state.StateName+"',"+state.MpSeats+")";
                SqlCommand cmd = new SqlCommand(strCmd, _cnn);
                cmd.CommandType = CommandType.Text;

                _cnn.Open();
                cmd.ExecuteNonQuery();
                _cnn.Close();

            }
            else
            {
                //update States set StateName='KA',MpSeats=150 where StateId=1
                string strCmd = @"update States set StateName='"+ state.StateName + "',MpSeats="+ state.MpSeats + " where StateId="+state.StateId+"";

                SqlCommand cmd = new SqlCommand(strCmd, _cnn);
                cmd.CommandType = CommandType.Text;

                _cnn.Open();
                cmd.ExecuteNonQuery();
                _cnn.Close();
            }
        }

        public States GetState(int stateId)
        {
            var state = new States();

            string strCmd = @"select * from States where StateId='"+ stateId + "'";

            SqlDataAdapter da = new SqlDataAdapter(strCmd, _cnn);
            da.SelectCommand.CommandType = CommandType.Text;

            DataSet DS = new DataSet();
            da.Fill(DS);
            DataTable dt = DS.Tables[0];

            if (dt.Rows.Count > 0)
            {
                state.StateId = Convert.ToInt32(dt.Rows[0]["StateId"]);
                state.StateName = dt.Rows[0]["StateName"].ToString();
                state.MpSeats = Convert.ToInt32(dt.Rows[0]["MpSeats"]);                
            }

            return state;
        }

        public List<Symbol> LoadSymbols()
        {
            var symbols = new List<Symbol>();

            string strCmd = @"select * from Symbols";

            SqlDataAdapter da = new SqlDataAdapter(strCmd, _cnn);
            da.SelectCommand.CommandType = CommandType.Text;

            DataSet DS = new DataSet();
            da.Fill(DS);
            DataTable dt = DS.Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var _symbol = new Symbol()
                {
                    SymbolId = Convert.ToInt32(dt.Rows[i]["SymbolId"]),
                    SymbolName = dt.Rows[i]["SymbolName"].ToString()                    
                };
                symbols.Add(_symbol);
            }

            return symbols;
        }

        public Symbol GetSymbol(int symbolId)
        {
            var symbol = new Symbol();

            string strCmd = @"select * from Symbols where SymbolId='" + symbolId + "'";

            SqlDataAdapter da = new SqlDataAdapter(strCmd, _cnn);
            da.SelectCommand.CommandType = CommandType.Text;

            DataSet DS = new DataSet();
            da.Fill(DS);
            DataTable dt = DS.Tables[0];

            if (dt.Rows.Count > 0)
            {
                symbol.SymbolId = Convert.ToInt32(dt.Rows[0]["SymbolId"]);
                symbol.SymbolName = dt.Rows[0]["SymbolName"].ToString();                
            }
            return symbol;
        }

        public void SaveSymbol(Symbol symbol)
        {
            if (symbol.SymbolId == 0)
            {

                string strCmd = @"insert into Symbols values ('" + symbol.SymbolName + "')";
                SqlCommand cmd = new SqlCommand(strCmd, _cnn);
                cmd.CommandType = CommandType.Text;

                _cnn.Open();
                cmd.ExecuteNonQuery();
                _cnn.Close();
            }
            else
            {                
                string strCmd = @"update Symbols set SymbolName='" + symbol.SymbolName + "' where SymbolId=" + symbol.SymbolId + "";

                SqlCommand cmd = new SqlCommand(strCmd, _cnn);
                cmd.CommandType = CommandType.Text;

                _cnn.Open();
                cmd.ExecuteNonQuery();
                _cnn.Close();
            }
        }

         */

        public Partie LoadpartyDetails(int id)
        {
            var partie = new Partie();

            string strCmd = "select P.PartyId,P.PartyName,P.SymbolId,Sy.SymbolName from Parties as P join Symbols as Sy on P.SymbolId=Sy.SymbolId where P.PartyId=@PartyId";

            using (var command = new SqliteCommand(strCmd, _connection))
            {
                command.Parameters.AddWithValue("@PartyId", id);
                _connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        partie.PartyId = reader.GetInt32(reader.GetOrdinal("PartyId"));
                        partie.PartyName = reader.GetString(reader.GetOrdinal("PartyName"));
                        partie.SymbolId = reader.GetInt32(reader.GetOrdinal("SymbolId"));
                        partie.PartiSymbol = new Symbol()
                        {
                            SymbolId = reader.GetInt32(reader.GetOrdinal("SymbolId")),
                            SymbolName = reader.GetString(reader.GetOrdinal("SymbolName"))
                        };
                    }
                }
            }

            return partie;
        }

        public List<Voter> Loadvoters()
        {
            var voters = new List<Voter>();

            string strCmd = "select * from Voters";

            using (var command = new SqliteCommand(strCmd, _connection))
            {
                _connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _voter = new Voter()
                        {
                            VoterId = reader.GetInt32(reader.GetOrdinal("VoterId")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Address = reader.GetString(reader.GetOrdinal("Address")),
                            Photo = reader.GetString(reader.GetOrdinal("Photo")),
                            Status = reader.GetBoolean(reader.GetOrdinal("Status")),
                            StateId = reader.GetInt32(reader.GetOrdinal("StateId"))
                        };
                        voters.Add(_voter);
                    }
                }
            }

            return voters;
        }

        public void SaveVoter(Voter voter)
        {
            string strCmd;
            if (voter.VoterId == 0)
            {
                strCmd = "insert into Voters (Name, Address, Photo, Status, StateId) values (@Name, @Address, @Photo, @Status, @StateId)";
            }
            else
            {
                strCmd = "update Voters set Name=@Name, Address=@Address, Photo=@Photo, Status=@Status, StateId=@StateId where VoterId=@VoterId";
            }

            using (var command = new SqliteCommand(strCmd, _connection))
            {
                command.Parameters.AddWithValue("@Name", voter.Name);
                command.Parameters.AddWithValue("@Address", voter.Address);
                command.Parameters.AddWithValue("@Photo", voter.Photo);
                command.Parameters.AddWithValue("@Status", voter.Status);
                command.Parameters.AddWithValue("@StateId", voter.StateId);
                if (voter.VoterId != 0)
                {
                    command.Parameters.AddWithValue("@VoterId", voter.VoterId);
                }

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
        }

        public Voter GetVoter(int voterId)
        {
            var voter = new Voter();

            string strCmd = "select * from Voters where VoterId=@VoterId";

            using (var command = new SqliteCommand(strCmd, _connection))
            {
                command.Parameters.AddWithValue("@VoterId", voterId);
                _connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        voter.VoterId = reader.GetInt32(reader.GetOrdinal("VoterId"));
                        voter.Name = reader.GetString(reader.GetOrdinal("Name"));
                        voter.Address = reader.GetString(reader.GetOrdinal("Address"));
                        voter.Photo = reader.GetString(reader.GetOrdinal("Photo"));
                        voter.Status = reader.GetBoolean(reader.GetOrdinal("Status"));
                        voter.StateId = reader.GetInt32(reader.GetOrdinal("StateId"));
                    }
                }
            }

            return voter;
        }

        public Candidate Loadcandidatedetails(int _candidateId)
        {
            var candidate = new Candidate();

            string strCmd = "select c.CandidateId,c.Name,c.StateId,c.PartyId,p.PartyName,s.StateName from Candidates c join Parties as p on c.PartyId=p.PartyId join States s on c.StateId=s.StateId where c.CandidateId=@CandidateId";

            using (var command = new SqliteCommand(strCmd, _connection))
            {
                command.Parameters.AddWithValue("@CandidateId", _candidateId);
                _connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        candidate.CandidateId = reader.GetInt32(reader.GetOrdinal("CandidateId"));
                        candidate.Name = reader.GetString(reader.GetOrdinal("Name"));
                        candidate.PartyId = reader.GetInt32(reader.GetOrdinal("PartyId"));
                        candidate.StateId = reader.GetInt32(reader.GetOrdinal("StateId"));
                        candidate.PartyName = reader.GetString(reader.GetOrdinal("PartyName"));
                        candidate.StateName = reader.GetString(reader.GetOrdinal("StateName"));
                    }
                }
            }

            return candidate;
        }

        public void SaveCandidate(Candidate candidate)
        {
            string strCmd;
            if (candidate.CandidateId == 0)
            {
                strCmd = "insert into Candidates (Name, PartyId, StateId) values (@Name, @PartyId, @StateId)";
            }
            else
            {
                strCmd = "update Candidates set Name=@Name, PartyId=@PartyId, StateId=@StateId where CandidateId=@CandidateId";
            }

            using (var command = new SqliteCommand(strCmd, _connection))
            {
                command.Parameters.AddWithValue("@Name", candidate.Name);
                command.Parameters.AddWithValue("@PartyId", candidate.PartyId);
                command.Parameters.AddWithValue("@StateId", candidate.StateId);
                if (candidate.CandidateId != 0)
                {
                    command.Parameters.AddWithValue("@CandidateId", candidate.CandidateId);
                }

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
        }

        public List<Symbol> GetSymbols()
        {
            var symbols = new List<Symbol>();

            string strCmd = "select * from Symbols";

            using (var command = new SqliteCommand(strCmd, _connection))
            {
                _connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _symbol = new Symbol()
                        {
                            SymbolId = reader.GetInt32(reader.GetOrdinal("SymbolId")),
                            SymbolName = reader.GetString(reader.GetOrdinal("SymbolName"))
                        };
                        symbols.Add(_symbol);
                    }
                }
            }

            return symbols;
        }

        public void SaveParty(Partie partie)
        {
            string strCmd;
            if (partie.PartyId == 0)
            {
                strCmd = "insert into Parties (PartyName, SymbolId) values (@PartyName, @SymbolId)";
            }
            else
            {
                strCmd = "update Parties set PartyName=@PartyName, SymbolId=@SymbolId where PartyId=@PartyId";
            }

            using (var command = new SqliteCommand(strCmd, _connection))
            {
                command.Parameters.AddWithValue("@PartyName", partie.PartyName);
                command.Parameters.AddWithValue("@SymbolId", partie.SymbolId);
                if (partie.PartyId != 0)
                {
                    command.Parameters.AddWithValue("@PartyId", partie.PartyId);
                }

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
        }

        public List<States> GetStates()
        {
            var states = new List<States>();

            string strCmd = "select * from States";

            using (var command = new SqliteCommand(strCmd, _connection))
            {
                _connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _states = new States()
                        {
                            StateId = reader.GetInt32(reader.GetOrdinal("StateId")),
                            StateName = reader.GetString(reader.GetOrdinal("StateName")),
                            MpSeats = reader.GetInt32(reader.GetOrdinal("MpSeats")),
                        };
                        states.Add(_states);
                    }
                }
            }

            return states;
        }

        public void SaveState(States state)
        {
            string strCmd;
            if (state.StateId == 0)
            {
                strCmd = "insert into States (StateName, MpSeats) values (@StateName, @MpSeats)";
            }
            else
            {
                strCmd = "update States set StateName=@StateName, MpSeats=@MpSeats where StateId=@StateId";
            }

            using (var command = new SqliteCommand(strCmd, _connection))
            {
                command.Parameters.AddWithValue("@StateName", state.StateName);
                command.Parameters.AddWithValue("@MpSeats", state.MpSeats);
                if (state.StateId != 0)
                {
                    command.Parameters.AddWithValue("@StateId", state.StateId);
                }

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
        }

        public States GetState(int stateId)
        {
            var state = new States();

            string strCmd = "select * from States where StateId=@StateId";

            using (var command = new SqliteCommand(strCmd, _connection))
            {
                command.Parameters.AddWithValue("@StateId", stateId);
                _connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        state.StateId = reader.GetInt32(reader.GetOrdinal("StateId"));
                        state.StateName = reader.GetString(reader.GetOrdinal("StateName"));
                        state.MpSeats = reader.GetInt32(reader.GetOrdinal("MpSeats"));
                    }
                }
            }

            return state;
        }

        public List<Symbol> LoadSymbols()
        {
            var symbols = new List<Symbol>();

            string strCmd = "select * from Symbols";

            using (var command = new SqliteCommand(strCmd, _connection))
            {
                _connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _symbol = new Symbol()
                        {
                            SymbolId = reader.GetInt32(reader.GetOrdinal("SymbolId")),
                            SymbolName = reader.GetString(reader.GetOrdinal("SymbolName"))
                        };
                        symbols.Add(_symbol);
                    }
                }
            }

            return symbols;
        }

        public Symbol GetSymbol(int symbolId)
        {
            var symbol = new Symbol();

            string strCmd = "select * from Symbols where SymbolId=@SymbolId";

            using (var command = new SqliteCommand(strCmd, _connection))
            {
                command.Parameters.AddWithValue("@SymbolId", symbolId);
                _connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        symbol.SymbolId = reader.GetInt32(reader.GetOrdinal("SymbolId"));
                        symbol.SymbolName = reader.GetString(reader.GetOrdinal("SymbolName"));
                    }
                }
            }

            return symbol;
        }

        public void SaveSymbol(Symbol symbol)
        {
            string strCmd;
            if (symbol.SymbolId == 0)
            {
                strCmd = "insert into Symbols (SymbolName) values (@SymbolName)";
            }
            else
            {
                strCmd = "update Symbols set SymbolName=@SymbolName where SymbolId=@SymbolId";
            }

            using (var command = new SqliteCommand(strCmd, _connection))
            {
                command.Parameters.AddWithValue("@SymbolName", symbol.SymbolName);
                if (symbol.SymbolId != 0)
                {
                    command.Parameters.AddWithValue("@SymbolId", symbol.SymbolId);
                }

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
        }


    }
}
