using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public partial class card : baseModel
    {
        public int TotalRows { get; set; }
        public CardPagination get_operator(int page = 1, int pagesize = 100, string taxi = null, string gafet = null, string Name = null, string lastName1 = null, string lastName2 = null)
        {
            try
            {
                taxi = taxi == "" || taxi is null ? "null" : "'" + taxi + "'";
                gafet = gafet == "" || gafet is null ? "null" : "'" + gafet + "'";
                Name = Name == "" || Name is null ? "null" : "'" + Name + "'";
                lastName1 = lastName1 == "" || lastName1 is null ? "null" : "'" + lastName1 + "'";
                lastName2 = lastName2 == "" || lastName2 is null ? "null" : "'" + lastName2 + "'";
                string query = "fdo_get_cards " + page + ", " + pagesize + ", " + taxi + ", " + gafet + ", " + Name + ", " + lastName1 + ", " + lastName2;
                List<card> Cards = this.db.Database.SqlQuery<card>(query).ToList();
                int NumberOfClients = 0;
                if (Cards.Count > 0) NumberOfClients = (int)Cards.FirstOrDefault().TotalRows;
                int TotalPages = (int)Math.Ceiling((double)NumberOfClients / pagesize);
                CardPagination clientPagination = new CardPagination(NumberOfClients, Cards, page, pagesize);
                return clientPagination;
            }
            catch (Exception ex)
            {

                return null;
            }
            
        }
        public CardPagination get_partner(int page = 1, int pagesize = 100, string taxi = null, string gafet = null, string Name = null, string lastName1 = null, string lastName2 = null)
        {
            try
            {
                taxi = taxi == "" || taxi is null ? "null" : "'" + taxi + "'";
                gafet = gafet == "" || gafet is null ? "null" : "'" + gafet + "'";
                Name = Name == "" || Name is null ? "null" : "'" + Name + "'";
                lastName1 = lastName1 == "" || lastName1 is null ? "null" : "'" + lastName1 + "'";
                lastName2 = lastName2 == "" || lastName2 is null ? "null" : "'" + lastName2 + "'";
                string query = "fdo_get_cards_partners " + page + ", " + pagesize + ", " + taxi + ", " + gafet + ", " + Name + ", " + lastName1 + ", " + lastName2;
                List<card> Cards = this.db.Database.SqlQuery<card>(query).ToList();
                int NumberOfClients = 0;
                if (Cards.Count > 0) NumberOfClients = (int)Cards.FirstOrDefault().TotalRows;
                int TotalPages = (int)Math.Ceiling((double)NumberOfClients / pagesize);
                CardPagination clientPagination = new CardPagination(NumberOfClients, Cards, page, pagesize);
                return clientPagination;
            }
            catch (Exception ex)
            {

                return null;
            }

        }

        public List<card> get()
        {
            try
            {
                return this.db.cards.ToList(); // Obtenemos todos los tarjetones.
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public List<card> get(string gafet)
        {
            try
            {
                using (var db = new radiotaxiEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    db.Configuration.ProxyCreationEnabled = false;
                    return db.cards.Where(x => x.partnerReference == gafet).ToList(); // Obtenemos todos los tarjetones.
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public card get(int id)
        {
            try
            {
                card Card = this.db.cards.Where(x => x.id == id).ToList().FirstOrDefault(); // Obtenemos todos los tarjetones.
                Card.partnerReference = Card.partnerReference.ToUpper();
                return Card;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public card Save() {
            try
            {
                this.expiration_date = DateTime.Now.AddYears(1).AddDays(15);
                this.partnerReference = this.partnerReference.ToUpper();
                this.db.cards.Add(this); // Agregamos el objeto.
                this.db.SaveChanges(); // Guardamos Cambios en la base de datos.
                return this;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public card Update()
        {
            try
            {
                this.db.Entry<card>(this).State = EntityState.Modified; // Modificacion. 
                this.db.SaveChanges();
                return this;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public card Delete()
        {
            try
            {
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
