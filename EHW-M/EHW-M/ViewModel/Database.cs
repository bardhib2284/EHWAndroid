using EHW_M;
using EHWM.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHWM.ViewModel
{
    public class Database {
        public readonly SQLiteAsyncConnection _database;

        public Database(string dbPath) {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Agjendet>();
            _database.CreateTableAsync<KlientDheLokacion>();
            _database.CreateTableAsync<Vizita>();
            _database.CreateTableAsync<Artikulli>();
            _database.CreateTableAsync<SalesPrice>();
            _database.CreateTableAsync<Malli_Mbetur>();
            _database.CreateTableAsync<NumriFaturave>();
            _database.CreateTableAsync<Liferimi>();
            _database.CreateTableAsync<LiferimiArt>();
            _database.CreateTableAsync<Porosite>();
            _database.CreateTableAsync<PorosiaArt>();
            _database.CreateTableAsync<Stoqet>();
            _database.CreateTableAsync<NumriFisk>();
            _database.CreateTableAsync<SyncConfiguration>();
            _database.CreateTableAsync<CashRegister>();
            _database.CreateTableAsync<Konfigurimi>();
            _database.CreateTableAsync<Klientet>();
            _database.CreateTableAsync<LogSync_Session>();
            _database.CreateTableAsync<Configurimi>();
            _database.CreateTableAsync<Vendet>();
            _database.CreateTableAsync<StatusiVizites>();
            _database.CreateTableAsync<EvidencaPagesave>();
            _database.CreateTableAsync<CompanyInfo>();
            _database.CreateTableAsync<Orders>();
            _database.CreateTableAsync<Depot>();
            _database.CreateTableAsync<OrderDetails>();
            _database.CreateTableAsync<LevizjetHeader>();
            _database.CreateTableAsync<LevizjetDetails>();
            _database.CreateTableAsync<Detyrimet>();
            _database.CreateTableAsync<KrijimiPorosive>();
            _database.CreateTableAsync<FiskalizimiKonfigurimet>();
            _database.CreateTableAsync<Arsyejet>();
            _database.CreateTableAsync<NumriPorosive>();
            _database.CreateTableAsync<Linqet>();
        }

        public async Task<List<Table>> QueryAsync(string query) {
            var result = await _database.QueryAsync<Table>(query);
            return result;
        }

        public async Task<bool> TableExists(string TableName) {
            var result = await _database.QueryAsync<Table>("SELECT name FROM sqlite_master WHERE type='table' AND name='"+ TableName +"'");
            return result.Count > 0;
        }

        public async Task<bool> DropTable(string TableName) {
            var result = await _database.QueryAsync<Table>("drop table " + TableName );
            return result.Count > 0;
        }

        public async Task<int> ClearAllFiskalizimiKonfigurimi() {
            return await _database.DeleteAllAsync<FiskalizimiKonfigurimet>();
        }
        public async Task<int> ClearAllNumratFiskalAsync() {
            return await _database.DeleteAllAsync<NumriFisk>();
        }
        public async Task<int> ClearAllCashRegistersAsync() {
            return await _database.DeleteAllAsync<CashRegister>();
        }
        public async Task<int> ClearAllLevizjetDetailsAsync() {
            return await _database.DeleteAllAsync<LevizjetDetails>();
        }
        public async Task<int> ClearAllLevizjetHeaderAsync() {
            return await _database.DeleteAllAsync<LevizjetHeader>();
        }
        public async Task<int> ClearAllOrderDetailsAsync() {
            return await _database.DeleteAllAsync<OrderDetails>();
        }
        public async Task<int> ClearAllEvidencaPagesave() {
            return await _database.DeleteAllAsync<EvidencaPagesave>();
        }
        public async Task<int> ClearAllAgjendetAsync() {
            return await _database.DeleteAllAsync<Agjendet>();
        }
        public async Task<int> ClearAllStatusiVizitesAsync() {
            return await _database.DeleteAllAsync<StatusiVizites>();
        }
        public async Task<int> ClearAllKlientetDheLokacion() {
            return await _database.DeleteAllAsync<KlientDheLokacion>();
        }

        public async Task<int> ClearAllDetyrimetAsync() {
            return await _database.DeleteAllAsync<Detyrimet>();
        }
        public async Task<int> ClearAllDepotAsync() {
            return await _database.DeleteAllAsync<Depot>();
        }
        public async Task<int> ClearAllCompanyInfo() {
            return await _database.DeleteAllAsync<CompanyInfo>();
        }
        
        public async Task<int> DeleteVizita(Vizita viz) {
            return await _database.DeleteAsync(viz);
        }
        
        public async Task<int> ClearAllVizitat() {
            return await _database.DeleteAllAsync<Vizita>();
        }
        
        public async Task<int> ClearAllNumriFaturaveAsync() {
            return await _database.DeleteAllAsync<NumriFaturave>();
        }          

        public async Task<int> ClearAllKonfigurimet() {
            return await _database.DeleteAllAsync<Konfigurimi>();
        }     
        
        public async Task<int> ClearAllSalesPrice() {
            return await _database.DeleteAllAsync<SalesPrice>();
        } 
        public async Task<int> ClearAllStoqetAsync() {
            return await _database.DeleteAllAsync<Stoqet>();
        }     
        
        public async Task<int> ClearAllSyncConfigs() {
            return await _database.DeleteAllAsync<SyncConfiguration>();
        }  
        public async Task<int> ClearAllKlientet() {
            return await _database.DeleteAllAsync<Klientet>();
        }    
        
        public async Task<int> ClearAllKrijimiPorosive() {
            return await _database.DeleteAllAsync<KrijimiPorosive>();
        }    
        
        public async Task<int> ClearAllVendetAsync() {
            return await _database.DeleteAllAsync<Vendet>();
        }        
        public async Task<int> ClearAllOrdersAsync() {
            return await _database.DeleteAllAsync<Orders>();
        }        
        public async Task<int> ClearAllArsyejetAsync() {
            return await _database.DeleteAllAsync<Arsyejet>();
        }       
        public async Task<int> ClearAllArtikujtAsync() {
            return await _database.DeleteAllAsync<Artikulli>();
        }         

        public async Task<int> ClearAlFiskalizimiKonfigurimet() {
            return await _database.DeleteAllAsync<FiskalizimiKonfigurimet>();
        } 
        public async Task<int> ClearAlKrijimiPorosive() {
            return await _database.DeleteAllAsync<KrijimiPorosive>();
        }  

        public async Task<int> ClearAllPorositeArt() {
            return await _database.DeleteAllAsync<PorosiaArt>();
        } 
        public async Task<int> ClearAllPorosite() {
            return await _database.DeleteAllAsync<Porosite>();
        } 
        public async Task<int> ClearAllLiferimetArt() {
            return await _database.DeleteAllAsync<LiferimiArt>();
        }  
        public async Task<int> ClearAllLiferimet() {
            return await _database.DeleteAllAsync<Liferimi>();
        }        
        public async Task<int> ClearAllMalliMbetur() {
            return await _database.DeleteAllAsync<Malli_Mbetur>();
        }
        
        public async Task<bool> UpdateStatusField(string tableName, string statusField, string CurrentValue, string updateValue) {
            var result = await _database.QueryAsync<Table>(String.Format("Update {0} set {1}={2} where {1}={3}", tableName, statusField, updateValue, CurrentValue));
            return result.Count > 0;
        }

        public bool IsDBConnected() {
            var res = _database.GetConnection();
            return res != null;
        }

        public async Task<List<FiskalizimiKonfigurimet>> GetFiskalizimiKonfigurimetAsync() {
            return await _database.Table<FiskalizimiKonfigurimet>().ToListAsync();
        }
        public async Task<List<KrijimiPorosive>> GetKrijimiPorosiveAsync() {
            return await _database.Table<KrijimiPorosive>().ToListAsync();
        }
        public async Task<List<Detyrimet>> GetDetyrimetAsync() {
            return await _database.Table<Detyrimet>().ToListAsync();
        }
        public async Task<List<LevizjetDetails>> GetLevizjeDetailsAsync() {
            return await _database.Table<LevizjetDetails>().ToListAsync();
        }
        public async Task<List<LevizjetHeader>> GetLevizjetHeaderAsync() {
            return await _database.Table<LevizjetHeader>().ToListAsync();
        }
        public async Task<List<OrderDetails>> GetOrderDetailsAsync() {
            return await _database.Table<OrderDetails>().ToListAsync();
        }
        public async Task<List<Orders>> GetOrdersAsync() {
            return await _database.Table<Orders>().ToListAsync();
        }
        public async Task<List<EvidencaPagesave>> GetEvidencaPagesaveAsync() {
            return await _database.Table<EvidencaPagesave>().ToListAsync();
        }
        public async Task<List<SyncConfiguration>> GetSyncConfigurationsAsync() {
            return await _database.Table<SyncConfiguration>().ToListAsync();
        }
        public async Task<List<Konfigurimi>> GetKonfigurimetAsync() {
            return await _database.Table<Konfigurimi>().ToListAsync();
        }

        public async Task<List<CompanyInfo>> GetCompanyInfoAsync() {
            return await _database.Table<CompanyInfo>().ToListAsync();
        }
        public async Task<List<CashRegister>> GetCashRegisterAsync() {
            return await _database.Table<CashRegister>().ToListAsync();
        }

        public async Task<Configurimi> GetConfigurimiAsync() {
            try {
                return await _database.Table<Configurimi>().FirstOrDefaultAsync();

            }
            catch (Exception ex) {
                return null;
            }
            
        }
        public async Task<List<StatusiVizites>> GetStatusiVizitesAsync() {
            try {
                return await _database.Table<StatusiVizites>().ToListAsync();

            }catch(Exception ex) {
                return null;
            }
        }

        public async Task<List<Arsyejet>> GetArsyejetAsync() {
            try {
                var res = await _database.Table<Arsyejet>().ToListAsync();
                return res;
            }catch(Exception ex) {
                return null;
            }
        }
        public async Task<List<Agjendet>> GetAgjendetAsync() {
            try {
                return await _database.Table<Agjendet>().ToListAsync();

            }catch(Exception ex) {
                return null;
            }
        }
        public async Task<Agjendet> GetSelectedAgjendiAsync(string IDAgjendi) {
            var list = await _database.Table<Agjendet>().ToListAsync();
            return list.FirstOrDefault(x => x.IDAgjenti == IDAgjendi);
        }
        public async Task<List<Klientet>> GetKlientetAsync() {
            return await _database.Table<Klientet>().ToListAsync();
        }

        public async Task<List<PorosiaArt>> GetPorositeArtAsyncWithPorosiaID(Guid PorosiaID) {
            var porositeArt =  await _database.Table<PorosiaArt>().ToListAsync();
            return porositeArt.Where(x => x.IDPorosia == PorosiaID).ToList();
        }
        public async Task<List<PorosiaArt>> GetPorositeArtAsync() {
            return await _database.Table<PorosiaArt>().ToListAsync();
        }
        public async Task<PorosiaArt> GetPorositaArtAsync(string deviceID,int sasiaPorositur) {
            var list = await _database.Table<PorosiaArt>().ToListAsync();
            return list.FirstOrDefault(x => x.DeviceID == deviceID && x.SasiaPorositur == sasiaPorositur);
        }

        public async Task<List<NumriFisk>> GetNumratFiskalAsync() {
            return await _database.Table<NumriFisk>().ToListAsync();
        }

        public async Task<NumriFisk> GetNumratFiskalIDAsync(string depo) {
            var list =  await _database.Table<NumriFisk>().ToListAsync();
            return list.FirstOrDefault(x => x.TCRCode == App.Instance.MainViewModel.Configurimi.KodiTCR);
        }
        public async Task<int> GetPorositeArtCountAsync(Guid porosiaId) {
            var list = await _database.Table<PorosiaArt>().ToListAsync();
            var listId = list.Where(x => x.IDPorosia == porosiaId);
            return list.Count;
        }

        public async Task<List<Porosite>> GetPorositeAsync() {
            return await _database.Table<Porosite>().ToListAsync();
        }
        public async Task<Porosite> GetPorositeIDAsync(Guid porosiaID) {
            var list = await _database.Table<Porosite>().ToListAsync();
            return list.FirstOrDefault(x => x.IDPorosia == porosiaID);
        }
        
        public async Task<Stoqet> GetStokuAsync(string shifra,string depo, string seri) {
            var list = await _database.Table<Stoqet>().ToListAsync();
            return list.FirstOrDefault(x => x.Depo == depo && x.Seri == seri);
        }

        
        public async Task<List<Depot>> GetDepotAsync() {
            return await _database.Table<Depot>().ToListAsync();
        }          
        public async Task<List<LiferimiArt>> GetLiferimetArtAsync() {
            return await _database.Table<LiferimiArt>().ToListAsync();
        }        
                
        public async Task<List<LiferimiArt>> GetLiferimetArtAsyncWithDeviceId(string deviceId) {
            return await _database.Table<LiferimiArt>().Where(x=>x.DeviceID == deviceId).ToListAsync();
        }                     
        public async Task<List<Liferimi>> GetLiferimetAsyncWithDeviceId(string deviceId) {
            return await _database.Table<Liferimi>().Where(x=>x.DeviceID == deviceId).ToListAsync();
        }                  
        public async Task<List<Liferimi>> GetLiferimetAsync() {
            return await _database.Table<Liferimi>().ToListAsync();
        }        

        public async Task<int> UpdateNumriPorosiveAsync(NumriPorosive lif) {
            return await _database.UpdateAsync(lif);
        }
        public async Task<int> UpdateLiferimiAsync(Liferimi lif) {
            return await _database.UpdateAsync(lif);
        }
        public async Task<List<Liferimi>> GetLiferimiAsync() {
            return await _database.Table<Liferimi>().ToListAsync();
        }
        public async Task<List<Malli_Mbetur>> GetMalliMbeturAsync() {
            return await _database.Table<Malli_Mbetur>().ToListAsync();
        }
        public async Task<List<Malli_Mbetur>> GetMalliMbeturAsyncWithDepo(string depo) {
            return await _database.Table<Malli_Mbetur>().Where(x=> x.Depo == depo).ToListAsync();
        }
        
        public async Task<Malli_Mbetur> GetMalliMbeturIDAsync(string seri,string depo,string idArtikulli)
        {
            var list = await _database.Table<Malli_Mbetur>().ToListAsync();
            return list.FirstOrDefault(x => x.Seri == seri && x.Depo == depo && x.IDArtikulli == idArtikulli);
        }
       
        public async Task<List<Vendet>> GetVendetAsync() {
            return await _database.Table<Vendet>().ToListAsync();
        }       
        public async Task<List<Artikulli>> GetArtikujtAsync() {
            return await _database.Table<Artikulli>().ToListAsync();
        }
        public async Task<List<Vizita>> GetVizitatAsyncWithDeviceID(string deviceID) {
            return await _database.Table<Vizita>().Where(x=> x.DeviceID == deviceID).ToListAsync();
        }
        public async Task<List<Vizita>> GetVizitatAsync() {
            return await _database.Table<Vizita>().ToListAsync();
        }
        public async Task<List<NumriPorosive>> GetNumriPorosiveAsync() {
            return await _database.Table<NumriPorosive>().ToListAsync();
        }
        public async Task<List<NumriFaturave>> GetNumriFaturaveAsync() {
            return await _database.Table<NumriFaturave>().ToListAsync();
        }
        public async Task<NumriFaturave> GetNumriFaturaveIDAsync(string idAgjenti) {
            var list = await _database.Table<NumriFaturave>().ToListAsync();
            return list.FirstOrDefault(x => x.KOD == idAgjenti);
        }

        public async Task<List<KlientDheLokacion>> GetKlientetDheLokacionetAsync() {
            return await _database.Table<KlientDheLokacion>().ToListAsync();
        }
        public async Task<List<SalesPrice>> GetSalesPriceAsync() {
            return await _database.Table<SalesPrice>().ToListAsync();
        }

        public async Task<int> UpdateKrijimiPorosiveAsync(KrijimiPorosive KrijimiPorosive) {
            return await _database.UpdateAsync(KrijimiPorosive);
        }
        public async Task<int> UpdateOrderAsync(Orders order) {
            return await _database.UpdateAsync(order);
        }
        public async Task<int> UpdateVizitaAsync(Vizita viz) {
            return await _database.UpdateAsync(viz);
        }
        public async Task<int> UpdatePorositeAsync(Porosite por) {
            return await _database.UpdateAsync(por);
        }
        public async Task<int> UpdatePorositeArtAsync(PorosiaArt por) {
            try{
                return await _database.UpdateAsync(por);
            }
            catch (Exception e) { return -1; }
        }
        public async Task<int> UpdateStoqetAsync(Stoqet stoqet) {
            return await _database.UpdateAsync(stoqet);
        }
        public async Task<int> UpdateAllMalliMbeturAsync(List<Malli_Mbetur> malliMbetur) {
            try {
                return await _database.UpdateAllAsync(malliMbetur);
            }
            catch(Exception e) {
                return -1;
            }
        }
        public async Task<int> UpdateMalliMbeturAsync(Malli_Mbetur malliMbetur) {
            try {
                return await _database.UpdateAsync(malliMbetur);

            }catch(Exception e) {
                return -1;
            }
        }
        public async Task<int> UpdateNumriFaturave(NumriFaturave nrFaturave) {
            return await _database.UpdateAsync(nrFaturave);
        }

        public async Task<AuthenticatedUser> GetAuthenticatedUserAsync(AuthenticatedUser authenticatedUser) {
            try {
                return await _database.GetAsync<AuthenticatedUser>(x => x.Username == authenticatedUser.Username && x.Password == authenticatedUser.Password);

            }catch(Exception e) {
                return null;
            }
        }


        public async Task<int> SaveOrUpdateFiskalizimiKonfigurimet(FiskalizimiKonfigurimet cr) {
            try {
                return await _database.InsertOrReplaceAsync(cr);
            }
            catch (Exception e) {
                return -1;
            }
        }
        public async Task<int> SaveOrUpdateKrijimiPorosive(KrijimiPorosive cr) {
            try {
                return await _database.InsertOrReplaceAsync(cr);
            }
            catch (Exception e) {
                return -1;
            }
        }
        public async Task<int> UpdateDetyrimi(Detyrimet cr) {
            try {
                return await _database.UpdateAsync(cr);
            }
            catch (Exception e) {
                return -1;
            }
        }
        public async Task<int> SaveOrUpdateDetyrimi(Detyrimet cr) {
            try {
                return await _database.InsertOrReplaceAsync(cr);
            }
            catch (Exception e) {
                return -1;
            }
        }

        public async Task<int> SaveAllDetyrimetAsync(List<Detyrimet> cr) {
            try {
                return await _database.InsertAllAsync(cr);
            }
            catch (Exception e) {
                return -1;
            }
        }
        public async Task<int> SaveAllLevizjeDetailsAsync(List<LevizjetDetails> cr) {
            try {
                return await _database.InsertAllAsync(cr);
            }
            catch (Exception e) {
                return -1;
            }
        }

        public async Task<int> SaveLevizjeDetailsAsync(LevizjetDetails cr) {
            try {
                return await _database.InsertOrReplaceAsync(cr);
            }
            catch (Exception e) {
                return -1;
            }
        }
        public async Task<int> UpdateLevizjeDetailsAsync(LevizjetDetails cr) {
            try {
                return await _database.UpdateAsync(cr);
            }
            catch (Exception e) {
                return -1;
            }
        }
        public async Task<int> UpdateLevizjeHeaderAsync(LevizjetHeader cr) {
            try {
                return await _database.UpdateAsync(cr);
            }
            catch (Exception e) {
                return -1;
            }
        }
        public async Task<int> SaveLevizjeHeaderAsync(LevizjetHeader cr) {
            try {
                return await _database.InsertOrReplaceAsync(cr);
            }
            catch (Exception e) {
                return -1;
            }
        }
        public async Task<int> SaveAllLevizjetHeadersAsync(List<LevizjetHeader> cr) {
            try {
                return await _database.InsertAllAsync(cr);
            }
            catch (Exception e) {
                return -1;
            }
        }

        public async Task<int> SaveAllOrderDetailsAsync(List<OrderDetails> cr) {
            try {
                return await _database.InsertAllAsync(cr);
            }
            catch (Exception e) {
                return -1;
            }
        }

        public async Task<int> UpdateOrderDetailsAsync(OrderDetails cr) {
            try {
                return await _database.UpdateAsync(cr);
            }
            catch (Exception e) {
                return -1;
            }
        }

        public async Task<int> SaveNumriPorosiveAsync(NumriPorosive cr) {
            try {
                return await _database.InsertAsync(cr);
            }
            catch (Exception e) {
                return -1;
            }
        }

        public async Task<int> SaveOrderDetailsAsync(OrderDetails cr) {
            try {
                return await _database.InsertAsync(cr);
            }
            catch (Exception e) {
                return -1;
            }
        }
        public async Task<int> SaveOrderAsync(Orders cr) {
            try {
                return await _database.InsertOrReplaceAsync(cr);
            }
            catch (Exception e) {
                return -1;
            }
        }
        public async Task<int> SaveOrdersAsync(List<Orders> cr) {
            try {
                return await _database.InsertOrReplaceAsync(cr);
            }
            catch (Exception e) {
                return -1;
            }
        }
        public async Task<int> UpdateCashRegisterAsync(CashRegister cr) {
            try {
                return await _database.UpdateAsync(cr);
            }
            catch (Exception e) {
                return -1;
            }
        }
        public async Task<int> SaveCashRegisterAsync(CashRegister cr) {
            try {
                return await _database.InsertOrReplaceAsync(cr);
            }
            catch (Exception e) {
                return -1;
            }
        }
        public async Task<int> SaveDepotAsync(List<Depot> Depot) {
            try {
                return await _database.InsertAllAsync(Depot);
            }
            catch(Exception e) {
                return -1;
            }
        }
        public async Task<int> SaveFiskalizimiKonfigurimetAsync(List<FiskalizimiKonfigurimet> CompanyInfo) {
            try {
                return await _database.InsertAllAsync(CompanyInfo);
            }
            catch(Exception e) {
                return -1;
            }
        }
        public async Task<int> SaveCompanyInfoAsync(List<CompanyInfo> CompanyInfo) {
            try {
                return await _database.InsertAllAsync(CompanyInfo);
            }
            catch(Exception e) {
                return -1;
            }
        }
        public async Task<int> SaveAgjendetAsync(List<Agjendet> agjendi) {
            try {
                return await _database.InsertAllAsync(agjendi);
            }
            catch(Exception e) {
                return -1;
            }
        }
        public async Task<int> SaveVendetAsync(List<Vendet> vendets) {
            try {
                return await _database.InsertAllAsync(vendets);
            }
            catch(Exception e) {
                return -1;
            }
        }
        public async Task<int> SaveArsyejetAsync(List<Arsyejet> artikujt) {
            try {
                return await _database.InsertAllAsync(artikujt);
            }
            catch(Exception e) {
                return -1;
            }
        }
        public async Task<int> SaveArtikujtAsync(List<Artikulli> artikujt) {
            try {
                return await _database.InsertAllAsync(artikujt);
            }
            catch(Exception e) {
                return -1;
            }
        }
        public async Task<int> UpdateArtikulliAsync(Artikulli a) {
            try {
                return await _database.UpdateAsync(a);
            }
            catch(Exception e) {
                return -1;
            }
        }
        public async Task<int> SaveArtikulliAsync(Artikulli a) {
            try {
                return await _database.InsertOrReplaceAsync(a);
            }
            catch(Exception e) {
                return -1;
            }
        }
        public async Task<int> SaveAllKrijimiPorosiveAsync(List<KrijimiPorosive> a) {
            try {
                return await _database.InsertAllAsync(a);
            }
            catch(Exception e) {
                return -1;
            }
        }
        public async Task<int> SaveAllPorositeArtAsync(List<PorosiaArt> a) {
            try {
                return await _database.InsertAllAsync(a);
            }
            catch(Exception e) {
                return -1;
            }
        }
        public async Task<int> SaveAllPorositeAsync(List<Porosite> a) {
            try {
                return await _database.InsertAllAsync(a);
            }
            catch(Exception e) {
                return -1;
            }
        }
        public async Task<int> SavePorositeAsync(Porosite a) {
            try {
                return await _database.InsertOrReplaceAsync(a);
            }
            catch(Exception e) {
                return -1;
            }
        }

        public async Task<int> SaveLinkuAsync(Linqet a) {
            try {
                return await _database.InsertAsync(a);
            }
            catch(Exception e) {
                return -1;
            }
        }

        public async Task<int> SaveLiferimetAsync(List<Liferimi> a) {
            try {
                return await _database.InsertAllAsync(a);
            }
            catch(Exception e) {
                return -1;
            }
        }

        public async Task<int> UpdateLinkuAsync(Linqet a) {
            try {
                return await _database.UpdateAsync(a);
            }
            catch(Exception e) {
                return -1;
            }
        }
        public async Task<int> UpdateLiferimiArtAsync(LiferimiArt a) {
            try {
                return await _database.UpdateAsync(a);
            }
            catch(Exception e) {
                return -1;
            }
        }
        public async Task<int> SaveLiferimiArtAsync(LiferimiArt a) {
            try {
                return await _database.InsertAsync(a);
            }
            catch(Exception e) {
                return -1;
            }
        }
        public async Task<int> SaveKrijimiPorosiveAsync(KrijimiPorosive a) {
            try {
                return await _database.InsertAsync(a);
            }
            catch(Exception e) {
                return -1;
            }
        }
        public async Task<int> SaveLiferimiAsync(Liferimi a) {
            try {
                return await _database.InsertOrReplaceAsync(a);
            }
            catch(Exception e) {
                return -1;
            }
        }

        public async Task<int> UpdateNumriFiskalAsync(NumriFisk a) {
            try {
                return await _database.UpdateAsync(a);
            }
            catch(Exception e) {
                return -1;
            }
        }

        public async Task<int> SaveNumratFiskalAsync(List<NumriFisk> a) {
            try {
                return await _database.InsertAllAsync(a);
            }
            catch(Exception e) {
                return -1;
            }
        }
        public async Task<int> SaveNumriFiskalAsync(NumriFisk a) {
            try {
                return await _database.InsertOrReplaceAsync(a);
            }
            catch(Exception e) {
                return -1;
            }
        }
        public async Task<int> SaveNumriFaturaveAllAsync(List<NumriFaturave> a) {
            try {
                return await _database.InsertAllAsync(a);
            }
            catch(Exception e) {
                return -1;
            }
        }
        public async Task<int> SaveNumriFaturaveAsync(NumriFaturave a) {
            try {
                return await _database.InsertOrReplaceAsync(a);
            }
            catch(Exception e) {
                return -1;
            }
        }

        public async Task<int> SaveMalletEMbeturaAsync(List<Malli_Mbetur> a) {
            try {
                return await _database.InsertAllAsync(a);
            }
            catch(Exception e) {
                return -1;
            }
        }

        public async Task<int> SaveMalleteMbeturaAsync(List<Malli_Mbetur> a) {
            try {
                return await _database.InsertAllAsync(a);
            }
            catch(Exception e) {
                return -1;
            }
        }
        public async Task<int> SaveMalliMbeturAsync(Malli_Mbetur a) {
            try {
                return await _database.InsertOrReplaceAsync(a);
            }
            catch(Exception e) {
                return -1;
            }
        }
        public async Task<int> SaveMalliMbeturNoReplaceAsync(Malli_Mbetur a) {
            try {
                return await _database.InsertAsync(a);
            }
            catch(Exception e) {
                return -1;
            }
        }

        public async Task<int> SaveSalesPricesAsync(List<SalesPrice> a) {
            try {
                return await _database.InsertAllAsync(a);
            }
            catch(Exception e) {
                return -1;
            }
        }

        public async Task<int> SaveSalesPriceAsync(SalesPrice a) {
            try {
                return await _database.InsertOrReplaceAsync(a);
            }
            catch(Exception e) {
                return -1;
            }
        }


        public async Task<int> SaveKlienteteAsync(List<Klientet> klientet) {
            try {
                return await _database.InsertAllAsync(klientet);

            }
            catch(Exception e) {
                return -1;
            }
        }

        public async Task<int> SaveKlientAsync(Klientet klientet) {
            try {
                return await _database.InsertOrReplaceAsync(klientet);

            }
            catch(Exception e) {
                return -1;
            }
        }

        public async Task<int> SaveVizitatAsync(List<Vizita> Vizita) {
            try {
                int ind = 0;
                foreach(var viz in Vizita) {
                    ind += await _database.InsertOrReplaceAsync(viz);
                }
                return ind;

            }
            catch (Exception e) {
                return -1;
            }
        }
        public async Task<int> SaveVizitaAsync(Vizita Vizita) {
            try {
                return await _database.InsertAsync(Vizita);

            }
            catch (Exception e) {
                return -1;
            }
        }

        public async Task<int> SaveLogSyncSession(LogSync_Session KDL) {
            try {
                return await _database.InsertOrReplaceAsync(KDL);

            }
            catch (Exception e) {
                return -1;
            }
        }

        public async Task<int> SaveKlientetDheLokacionet(List<KlientDheLokacion> KDL) {
            try {
                return await _database.InsertAllAsync(KDL);

            }
            catch (Exception e) {
                return -1;
            }
        }
        public async Task<int> SaveKlientiDheLokacioni(KlientDheLokacion KDL) {
            try {
                return await _database.InsertOrReplaceAsync(KDL);

            }
            catch (Exception e) {
                return -1;
            }
        }
        public async Task<int> SaveAuthenticatedUserAsync(AuthenticatedUser Vizita) {
            try {
                return await _database.InsertOrReplaceAsync(Vizita);

            }
            catch (Exception e) {
                return -1;
            }
        }

        public async Task<int> SaveKonfigurimiAsync(Konfigurimi config) {
            try {
                return await _database.InsertOrReplaceAsync(config);

            }
            catch (Exception e) {
                return -1;
            }
        }

        public async Task<int> SaveKonfigurimetAsync(List<Konfigurimi> configs) {
            try {
                return await _database.InsertAllAsync(configs);

            }
            catch (Exception e) {
                return -1;
            }
        }
        public async Task<int> DeleteLiferimi(Liferimi ep) {
            try {
                return await _database.DeleteAsync(ep);
            }catch(Exception e) {
                return -1;
            }
        }
        public async Task<int> DeleteEvidencaPagesave(EvidencaPagesave ep) {
            try {
                return await _database.DeleteAsync(ep);
            }catch(Exception e) {
                return -1;
            }
        }
        public async Task<int> DeleteAllSyncConfigs() {
            try {
                return await _database.DeleteAllAsync<SyncConfiguration>();
            }catch(Exception e) {
                return -1;
            }
        }

        public async Task<int> SaveConfigurimiAsync(Configurimi config) {
            try {
                return await _database.InsertOrReplaceAsync(config);
            }
            catch (Exception e) {
                return -1;
            }
        }
        public async Task<int> SaveSyncConfigs(List<SyncConfiguration> configs) {
            try {
                return await _database.InsertAllAsync(configs);

            }
            catch (Exception e) {
                return -1;
            }
        }

        public async Task<int> SaveAllEvidencaPagesaveAsync(List<EvidencaPagesave> porosiaArt) {
            try {
                return await _database.InsertAllAsync(porosiaArt);

            }
            catch (Exception e) {
                return -1;
            }
        }
        public async Task<int> SaveEvidencaPagesaveAsync(EvidencaPagesave porosiaArt) {
            try {
                return await _database.InsertOrReplaceAsync(porosiaArt);

            }
            catch (Exception e) {
                return -1;
            }
        }

        public async Task<int> SavePorosiaArtAsync(PorosiaArt porosiaArt) {
            try {
                return await _database.InsertOrReplaceAsync(porosiaArt);

            }
            catch (Exception e) {
                return -1;
            }
        }

        public async Task<List<Stoqet>> GetAllStoqetWithDeviceIDAsync(string deviceID) {
            try {
                var res = await _database.Table<Stoqet>().ToListAsync();
                return res.Where(x => x.Depo == deviceID).ToList();
            }
            catch(Exception e) {
                return null;
            }
        }
        public async Task<List<Linqet>> GetAllLinqetAsync() {
            try {
                return await _database.Table<Linqet>().ToListAsync();
            }
            catch(Exception e) {
                return null;
            }
        }
        public async Task<List<Stoqet>> GetAllStoqetAsync() {
            try {
                return await _database.Table<Stoqet>().ToListAsync();
            }
            catch(Exception e) {
                return null;
            }
        }
        public async Task<int> SaveAlllistOfStatusiVizitesAsync(List<StatusiVizites> stoqet) {
            try {
                return await _database.InsertAllAsync(stoqet);
            }
            catch(Exception e) {
                return -1;
            }
        }
        public async Task<int> SaveAllStoqetAsync(List<Stoqet> stoqet) {
            try {
                return await _database.InsertAllAsync(stoqet);
            }
            catch(Exception e) {
                return -1;
            }
        }
    }
}
