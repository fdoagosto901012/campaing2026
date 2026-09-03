using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public class Coupon : baseModel
    {
        public List<pagocupone> pagocupones { get; set; }
        public List<CuponesBloqueo> CuponesBloqueos { get; set; }
        public List<TarifaCupone> tarifaCupones { get; set; }
        public string sector { get; set; }
        public string perforacion { get; set; }
        public string numero { get; set; }
        public string pago { get; set; }

        public Coupon getRate(string numero, string sector, string perforacion) {
            try
            {
                string query = @"select* from PAGOCUPONES where sector = '"  + sector + "' and numero = " + numero + ";";
                List<pagocupone> _pagocupone = this.db.Database.SqlQuery<pagocupone>(query).ToList();
                query = @"select * from CuponesBloqueo where numero=" + numero + " and sector='" + sector + "' and status='BLOQUEADO';";
                List<CuponesBloqueo> _bloqueo = this.db.Database.SqlQuery<CuponesBloqueo>(query).ToList();
                query = @"select * from tarifacupones where sector='" + sector + "' and proveedor=202 and perforacion='" + perforacion + "';";
                List<TarifaCupone> _tarifa = this.db.Database.SqlQuery<TarifaCupone>(query).ToList();
                Coupon coupon = new Coupon();
                coupon.pagocupones = _pagocupone;
                coupon.CuponesBloqueos = _bloqueo;
                coupon.tarifaCupones = _tarifa;
                coupon.perforacion = perforacion;
                coupon.numero = numero;
                coupon.pago = pago;
                return coupon;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<string> perforaciones() {
			try
			{
				string query = @"select T.Perforacion from TarifaCupones as T 
								group by Perforacion
								order by T.Perforacion;";
                List<string> _perforacion = this.db.Database.SqlQuery<string>(query).ToList();
				return _perforacion;
            }
            catch (Exception ex)
			{
				return null;
			}
		}

        public List<string> Sectores()
        {
            try
            {
                string query = @"   select T.Sector from TarifaCupones as T
									group by Sector
									order by T.Sector;";
                List<string> _perforacion = this.db.Database.SqlQuery<string>(query).ToList();
                return _perforacion;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void pay(string numero, string id_proveedor, string sector, string perforacion) {
			try
			{
				string query = @"";
			}
			catch (Exception)
			{

				throw;
			}
		}


        public void pay(int cuponId, int userid)
        {
            try
            {
                var db = this.db;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void geta() {
            try
            {
                var queryable = this.db.cuponsGroupPrinteds
                                    .Where(x => x.Printed == true && x.Deliver == false)
                                    .Select(a => new cuponsGroupPrintedDTO
                                    {
                                        Id = a.Id,
                                        CreateBy = a.CreateBy,
                                        CreateDate = a.CreateDate,
                                        Printed = a.Printed,
                                        PrintedDate = a.PrintedDate,
                                        Active = a.Active,
                                        Deliver = a.Deliver,
                                        DeliverDate = a.DeliverDate,
                                        Cupons = a.Cupons.Select(t => new CuponDTO
                                        {
                                            Name = t.Name,
                                            EditedBy = t.EditedBy,
                                            EditedDate = t.EditedDate,
                                            @lock = t.@lock,
                                            active = t.active,
                                            Couponrate = new CouponrateDTO
                                            {
                                                Valor = t.Couponrate.Valor,
                                                CouponrateID = t.Couponrate.CouponrateID,
                                                active = t.Couponrate.active,
                                                CreateBy = t.Couponrate.CreateBy,
                                                Name = t.Couponrate.Name,
                                                Cupontype = t.Couponrate.Cupontype,
                                                Hotel = t.Couponrate.Hotel
                                            },
                                            cuponsGroupPrintedId = t.cuponsGroupPrintedId,
                                            CouponratesID = t.CouponratesID,
                                            CreateBy = t.CreateBy,
                                            CreateDate = t.CreateDate,
                                            CuponsID = t.CuponsID,
                                            UserID = t.UserID,
                                        }).ToList(),
                                    }).ToList();
                //return queryable;
            }
            catch (Exception ex)
            {

            }
        
        }
    
    }
}
