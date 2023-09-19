using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Domain.Common;
using BRMSAPI.Utility;
using Configuration;
using Creative.Core.Extention;

namespace BRMSAPI.Domain;

public class Bus : ISoftDeletedEntity
{
    public int BusId { get; set; }

    [CheckNumber(0, ErrorMessage = "Bus Type is Required")]
    public int BusType { get; set; }

    [NotMapped]
    public string BusTypeLabel
    {
        get
        {
            if (BusType > 0)
            {
                return ((BusTypeEnu)BusType).ToUtilString();
            }

            return "";

        }

    }

    [CheckNumber(0, ErrorMessage = "Manufactural is Required")]
    public int Manufactural { get; set; }

    [StringLength(50, MinimumLength = 2, ErrorMessage = "Bus Model contains fewer or more characters than expected. (2 to 50 characters are expected)")]
    [Required(AllowEmptyStrings = false)]
    public string BusModel { get; set; }

    [CheckNumber(0, ErrorMessage = "FuelType is Required")]
    public int FuelType { get; set; }

    [NotMapped]
    public string FuelTypeLabel
    {
        get
        {
            if (FuelType > 0)
            {
                return ((BusFuelType)FuelType).ToUtilString();
            }
            return "";
        }
      
    }

    [CheckNumber(0, ErrorMessage = "Seating Capacity is Required")]
    public int SeatingCapacity { get; set; }

    [StringLength(50, MinimumLength = 2, ErrorMessage = "DriverName contains fewer or more characters than expected. (2 to 50 characters are expected)")]
    [Required(AllowEmptyStrings = false)]
    public string DriverName { get; set; }

    public string DriverNumber { get; set; }

    [StringLength(15, MinimumLength = 6, ErrorMessage = "PlateNumber contains fewer or more characters than expected. (6 to 15 characters are expected)")]
    [Required(AllowEmptyStrings = false)]
    public string PlateNumber { get; set; }

    public string ServiceIds { get; set; }

    public int RegRouteId { get; set; }
    public RegRoute RegRoute { get; set; }

    public int Status { get; set; }

    [StringLength(50, MinimumLength = 2, ErrorMessage = "Additional Note contains fewer or more characters than expected. (2 to 50 characters are expected)")]
    public string AdditionalNote { get; set; }

    [CheckNumber(0, ErrorMessage = "Bus Color is Required")]
    public int BusColor { get; set; }

    [NotMapped]
    public string BusColorLabel
    {
        get
        {
            if (BusColor > 0)
            {
                return ((BusColor)BusColor).ToUtilString();
            }
            return "";
        }

    }

    public string DataCreated { get; set; }
    public bool Deleted { get; set ; }
}



//public class BusObj
//{
//    public int Id { get; set; }

//    [CheckNumber(0, ErrorMessage = "Bus Type is Required")]
//    public int BusType { get; set; }

//    public string BusTypeLabel
//    {
//        get
//        {
//            if (BusType > 0)
//            {
//                return ((BusTypeEnu)BusType).ToUtilString();
//            }

//            return "";

//        }

//    }

//    [CheckNumber(0, ErrorMessage = "Manufactural is Required")]
//    public int Manufactural { get; set; }

//    [StringLength(50, MinimumLength = 2, ErrorMessage = "Bus Model contains fewer or more characters than expected. (2 to 50 characters are expected)")]
//    [Required(AllowEmptyStrings = false)]
//    public string BusModel { get; set; }

//    [CheckNumber(0, ErrorMessage = "FuelType is Required")]
//    public int FuelType { get; set; }

//    public string FuelTypeLabel
//    {
//        get
//        {
//            if (FuelType > 0)
//            {
//                return ((BusFuelType)FuelType).ToUtilString();
//            }
//            return "";
//        }
//    }

//    [CheckNumber(0, ErrorMessage = "Seating Capacity is Required")]
//    public int SeatingCapacity { get; set; }

//    [StringLength(50, MinimumLength = 2, ErrorMessage = "DriverName contains fewer or more characters than expected. (2 to 50 characters are expected)")]
//    [Required(AllowEmptyStrings = false)]
//    public string DriverName { get; set; }

//    public string DriverNumber { get; set; }

//    [StringLength(15, MinimumLength = 6, ErrorMessage = "PlateNumber contains fewer or more characters than expected. (6 to 15 characters are expected)")]
//    [Required(AllowEmptyStrings = false)]
//    public string PlateNumber { get; set; }

//    public string ServiceIds { get; set; }

//    public int RegRouteId { get; set; }
//    public RegRoute RegRoute { get; set; }

//    public int Status { get; set; }

//    [StringLength(50, MinimumLength = 2, ErrorMessage = "Additional Note contains fewer or more characters than expected. (2 to 50 characters are expected)")]
//    public string AdditionalNote { get; set; }

//    [CheckNumber(0, ErrorMessage = "Bus Color is Required")]
//    public int BusColor { get; set; }

//    public string BusColorLabel
//    { get
//        {
//            if (BusColor > 0)
//            {
//                return ((BusColor)BusColor).ToUtilString();
//            }
//            return "";
//        }

//    }

//    public string DataCreated { get; set; }


//}

public class BusResObj
{
    public int BusId { get; set; }
    public bool IsSuccessful { get; set; }

    public ResponseObj ErrorMessage { get; set; }

}
