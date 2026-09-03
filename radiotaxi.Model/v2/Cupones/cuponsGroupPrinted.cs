using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public partial class cuponsGroupPrinted : baseModel
    {
        public bool update() {
            try
            {
                this.db.Entry(this).State = EntityState.Modified;
                this.db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool save() {
            try
            {
                this.db.cuponsGroupPrinteds.Add(this);
                this.db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        public List<cuponsGroupPrintedDTO> getNoPrinted() {
            try
            {
                //Regresamos los grupos que no se encuentran impresos
                var queryable = this.db.cuponsGroupPrinteds
                    .Where(x => x.Printed == false)

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
                return queryable;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
        public Dictionary<string, List<cuponsGroupPrintedDTO>> ordes()
        {
            try
            {
                // Obtener todos los grupos de ordenes activas.
                Dictionary<string, List<cuponsGroupPrintedDTO>> master = new Dictionary<string, List<cuponsGroupPrintedDTO>>();
                List<string> groups = this.db.cuponsGroupPrinteds
                .Where(x => x.GroupFolio != null && x.Active == true)
                .Select(x => x.GroupFolio)
                .Distinct()
                .ToList();
                foreach (var folio in groups)
                {
                    var queryable = this.db.cuponsGroupPrinteds
                    .Where(x => x.GroupFolio == folio && x.Active == true)
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
                    master.Add(folio,queryable);
                }
                //Regresamos los grupos que no se encuentran impresos
                return master;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
        public List<cuponsGroupPrintedDTO> ordes(string key)
        {
            try
            {
                // Obtener todos los grupos de ordenes activas.
                var queryable = this.db.cuponsGroupPrinteds
                    .Where(x => x.GroupFolio == key && x.Active == true)
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

                //Regresamos los grupos que no se encuentran impresos
                return queryable;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
        public List<cuponsGroupPrinted> ordesDisable(string key)
        {
            try
            {
                // Obtener todos los grupos de ordenes activas.
                List<cuponsGroupPrinted> queryable = this.db.cuponsGroupPrinteds
                    .Where(x => x.GroupFolio == key).ToList();
                foreach (var item in queryable)
                {
                    try
                    {
                        item.Active = false;
                        this.db.Entry(item).State = EntityState.Modified;
                        this.db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //Regresamos los grupos que no se encuentran impresos
                return queryable;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
        public List<cuponsGroupPrintedDTO> getPrinted(int top = 100)
        {
            try
            {
                //Regresamos los grupos que no se encuentran impresos
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
                return queryable;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
        public cuponsGroupPrinted get(int id) {
            try
            {
                return this.db.cuponsGroupPrinteds.Find(id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}