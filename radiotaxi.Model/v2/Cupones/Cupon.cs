using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public partial class Cupon : baseModel
    {
        
        public Cupon save()
        {
            try
            {
                return null;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public bool update()
        {
            try
            {
                db.Entry(this).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();  
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public Cupon delete()
        {
            try
            {
                return null;
            }
            catch (Exception)
            {

                return null;
            }
        }


        /// ACTIONES GENERALES
        public int? pay(string seller, List<Cupon> cupons, int userid, decimal amount)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // Filtrar los tickets 
                    List<Cupon> _cupons = new List<Cupon>();
                    CouponPayment payment = new CouponPayment();
                    foreach (Cupon cupon in cupons)
                    {
                        // Obtenemos los cupones validos.
                        var _cupon = db.Cupons
                            .Include(x => x.Couponrate)
                            .Where(y =>
                                y.active == true &&
                                y.Paid == false &&
                                y.CuponsID == cupon.CuponsID
                             )
                            .FirstOrDefault();
                        if (_cupon != null)
                        {
                            _cupons.Add(_cupon);
                        }
                    }

                    if (_cupons.Count() > 0) // Si existen cupones entonces procede a ejecutar todo.
                    {
                        // Crear encabezado del pago
                        
                        payment.userId = userid;
                        payment.TotalAmount = amount;
                        payment.CreatedBy = seller;
                        payment.CreatedDate = DateTime.Now;
                        payment.Active = true;
                        db.CouponPayments.Add(payment);
                        db.SaveChanges();

                        foreach (Cupon _cupon in _cupons)
                        {
                            // Que no exista.
                            if (_cupon == null)
                                throw new Exception("El cupón no existe.");
                            if (_cupon.CouponPaymentId != null)
                                throw new Exception("El cupón ya fue pagado.");
                            // Actualizar cupón
                            _cupon.CouponPaymentId = payment.CouponPaymentId;
                            _cupon.Paid = true;
                            _cupon.PaymentDate = DateTime.Now;
                            db.Entry(_cupon).State = EntityState.Modified;
                            db.SaveChanges();
                            // Crear detalle
                            CouponPaymentDetail detail = new CouponPaymentDetail();
                            detail.CouponPaymentId = payment.CouponPaymentId;
                            detail.CuponsId = _cupon.CuponsID;
                            detail.Amount = _cupon.Couponrate.Valor;
                            db.CouponPaymentDetails.Add(detail);
                            db.SaveChanges();
                        }
                    }
                    transaction.Commit();
                    return payment.CouponPaymentId;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    Message = ex.Message;

                    return null;
                }
            }
        }

        public List<Cupon> get()
        {
            try
            {
                return this.db.Cupons
                    .Include(x => x.Couponrate.Hotel)
                    .Include(x => x.Couponrate.Cupontype)
                    .ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Cupon get(int id)
        {
            try
            {
                return this.db.Cupons
                    .Where(x => x.CuponsID == id)
                    .Include(x => x.Couponrate.Hotel)
                    .Include(x => x.Couponrate.Cupontype)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Cupon> getlimit(int top)
        {
            try
            {
                return this.db.Cupons
                    .Include(x => x.Couponrate.Hotel)
                    .Include(x => x.Couponrate.Cupontype)
                    .Take(top)
                    .ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool save(CouponCreateDTO obj)
        {
            try
            {

                // Primero obtenemos la tarifa.
                Couponrate couponrate = this.db.Couponrates.Where(x => x.CouponrateID == obj.tarifa).FirstOrDefault();
                if (couponrate != null)
                {
                    // Creamos un nuevo grupo de impresion. 
                    cuponsGroupPrinted cuponsGroupPrinted = new cuponsGroupPrinted();
                    cuponsGroupPrinted.CreateBy = "Sistema";
                    cuponsGroupPrinted.CreateDate = DateTime.Now;
                    cuponsGroupPrinted.Active = true;
                    cuponsGroupPrinted.save(); // Este es el grupo de impresion y control de impresion de cupones.
                    string query = @"insert into Cupons ([Name],CreateBy,EditedBy, CouponratesID, cuponsGroupPrintedId)
                                SELECT 'NA','Sistemas','Sistemas'," + couponrate.CouponrateID + "," + cuponsGroupPrinted.Id + @"
                                FROM ( SELECT TOP (" + obj.cant + @") ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS n FROM sys.all_objects) AS numeros;";
                    this.db.Database.ExecuteSqlCommand(query);
                    Console.WriteLine(query);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public List<Cupon> search(SerchCuponsDTO obj)
        {
            try
            {
                // REALIZAMOS LA BUSQUEDA DE CUPONES SEGUN LOS PARAMETROS ENVIADOS DESDE EL FORMULARIO DE MANERA PARALELA Y ASINCRONA.
                List<Cupon> response = new List<Cupon>();

                Cupon cupon = this.db.Cupons.Find(obj.folio);

                response = this.db.Cupons.AsNoTracking()
                    .Include(x => x.Couponrate.Hotel)
                    .Include(x => x.Couponrate.Cupontype)
                    .Where(x =>
                        // Obtener Si se encuetra activo.
                        (x.active == true)
                        
                        &&
                        
                        // Escenario 3:
                        // Hotel + folio → serial_ex
                        (
                            // Este bloque valida que sea un cupon que ya fue impreso y que tenga un serial de impresion.
                            obj.hc != 0 &&
                            obj.folio != 0 &&
                            x.serial_ex != null &&
                            // Validamos que el cupon pertenezca al hotel y que el folio sea igual al serial de impresion.
                            x.Couponrate.HotelID == obj.hc &&
                            x.serial_ex == obj.folio
                        )

                        ||

                        // Escenario 1:
                        // Solo folio → CuponsID
                        (
                            obj.hc == 0 &&
                            obj.folio != 0 &&
                            x.CuponsID == obj.folio &&
                            x.serial_ex == null
                        )

                        ||

                        // Escenario 2:
                        // Hotel + folio → CuponsID
                        (
                            obj.hc != 0 &&
                            obj.folio != 0 &&
                            x.Couponrate.HotelID == obj.hc &&
                            x.CuponsID == obj.folio &&
                            x.serial_ex == null
                        )
                    )
                    .ToList();
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int DisplayFolio =>
            serial_ex != null
                ? (int)serial_ex
                : CuponsID;
    }
}
