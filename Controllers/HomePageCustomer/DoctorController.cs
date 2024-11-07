using Microsoft.AspNetCore.Mvc;

public class DoctorController : Controller
{
    public ActionResult Doctors()
    {
        return View();
    }
    public ActionResult DoctorDetails()
    {
        return View();
    }
}
