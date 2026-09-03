using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public partial class city : baseModel
    {


        public List<city> get() {
            return this.db.cities.OrderBy(x => x.name).ToList();
        }

        public List<city> get(int id)
        {
            try
            {
                return this.db.cities.Where(x => x.stateId == id).OrderBy(x => x.name).ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public city getcity(int id)
        {
            try
            {
                return this.db.cities.Where(x => x.id == id).OrderBy(x => x.name).ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public state getstate(int id)
        {
            try
            {
                return this.db.states.Where(x => x.id == id).OrderBy(x => x.name).ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }

}
