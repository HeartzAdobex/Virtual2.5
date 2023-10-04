using System.Xml.Linq;

namespace CalMall;

public partial class ResultPage : ContentPage
{

    public ResultPage(CalculationResult calculationResult)
	{
		InitializeComponent();
        ResultLabel.Text = $"พื้นที่ที่ดิน: {calculationResult.A} ตารางวา";
        RLabel.Text = $"อัตราส่วนพื้นที่อาคารต่อที่ดิน (ไม่รวมจอดรถ): {calculationResult.R} เท่า";
        FLabel.Text = $"พื้นที่อาคาร: {calculationResult.F}  ตารางวา";
        SLabel.Text = $"พื้นที่ขาย/ให้เช่า: {calculationResult.S}   ตารางวา";
        S_PathLabel.Text = $"พื้นที่ทางสัญจร: {calculationResult.S_Path}   ตารางวา";
        S_IndoorLabel.Text = $"พื้นที่ลานกิจกรรมในร่ม: {calculationResult.S_Indoor}  ตารางวา";
        S_GardenLabel.Text = $"พื้นที่สวนหย่อมในร่ม: {calculationResult.S_Garden}  ตารางวา";
        S_OfficeLabel.Text = $"พื้นที่สำนักงานและส่วนบริการ: {calculationResult.S_Office}   ตารางวา";
        S_ToiletLabel.Text = $"พื้นที่ห้องน้ำและห้องเครื่อง: {calculationResult.S_Toilet}   ตารางวา";
        cLabel.Text = $"จำนวนที่จอดรถ: {calculationResult.c}   ตารางวา";
        CLabel.Text = $"พื้นที่จอดรถ: {calculationResult.C}   ตารางวา";
        BCLabel.Text = $"พื้นที่คลุมดิน: {calculationResult.BC}   ตารางวา";
        OSALabel.Text = $"พื้นที่เปิดโล่ง: {calculationResult.OSA}   ตารางวา";
        GLabel.Text = $"พื้นที่สีเขียวยั่งยืน: {calculationResult.G}   ตารางวา";
        FARLabel.Text = $"อัตราส่วนพื้นที่อาคารต่อพื้นที่ดิน: {calculationResult.FAR}   ตารางวา";
        OSRLabel.Text = $"อัตราส่วนพื้นร้อยละพื้นที่เปิดโล่งต่อพื้นที่อาคาร: {calculationResult.OSR}   ตารางวา";

    }

}