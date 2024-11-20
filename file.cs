using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.Odbc;

namespace MvcMovie.Controllers;

public class HomeCtrl : Controller
{

    private readonly string _connectionString;

    public HomeCtrl(YourDbContext context)
    {
        _connectionString = configuration.GetConnectionString("YourConnectionStringName");
    }

    public IActionResult Test1(string filename)
    {
        if (string.IsNullOrEmpty(filename))
        {
            throw new ArgumentNullException("error");
        }
        string filepath = Path.Combine("/FILESHARE/images", filename);
        // ruleid: file-taint
        return File.ReadAllBytes(filepath);
    }

    public IActionResult Test2(string filename)
    {
        if (string.IsNullOrEmpty(filename))
        {
            throw new ArgumentNullException("error");
        }
        // ruleid: file-taint
        return System.IO.File.ReadAllBytes("/FILESHARE/images" + filename);
    }

    public IActionResult Test3(string filename)
    {
        if (string.IsNullOrEmpty(filename))
        {
            throw new ArgumentNullException("error");
        }

        string FolderPath = "/FILESHARE/images";

        MemoryStream memory = new MemoryStream();
        // ruleid: file-taint
        using (FileStream stream = new FileStream(FolderPath + filename, FileMode.Open))
        {
            await stream.CopyToAsync(memory);
        }
        memory.Position = 0;
        return File(memory, "image/png", "download");
    }

    public IActionResult OkTest1([FromBody] CmdBody body)
    {
        if (string.IsNullOrEmpty(filename))
        {
            throw new ArgumentNullException("error");
        }
        var filename = Path.GetFileName(body.filename);
        // ok: file-taint
        string filepath = Path.Combine("/FILESHARE/images", filename);
        return File.ReadAllBytes(filepath);
    }
    public IActionResult OkTest11([FromBody] CmdBody body)
    {
        if (string.IsNullOrEmpty(filename))
        {
            throw new ArgumentNullException("error");
        }
        var filename = Path.GetFileNameWithoutExtension(body.filename);
        // ok: file-taint
        string filepath = Path.Combine("/FILESHARE/images", filename);
        return File.ReadAllBytes(filepath);
    }

    public IActionResult OkTest2([FromBody] CmdBody body)
    {
        if (string.IsNullOrEmpty(body.filename) || Path.GetFileName(body.filename) != body.filename)
        {
            throw new ArgumentNullException("error");
        }
        string filepath = Path.Combine("/FILESHARE/images", body.filename);
        // ok: file-taint
        return File.ReadAllBytes(filepath);
    }

    [NonAction]
    public IActionResult OkTest3(CmdForm form)
    {
        if (string.IsNullOrEmpty(form.filename))
        {
            throw new ArgumentNullException("error");
        }
        string filepath = Path.Combine("/FILESHARE/images", form.filename);
        // ok: file-taint
        return File.ReadAllBytes(filepath);
    }
}

public class HomeController
{
    public IActionResult Test4(string filename)
    {
        if (string.IsNullOrEmpty(filename))
        {
            throw new ArgumentNullException("error");
        }
        string filepath = Path.Combine("/FILESHARE/images", filename);
        // ruleid: file-taint
        return File.ReadAllBytes(filepath);
    }

}

[Controller]
public class ThisIsCtrller
{
    public IActionResult Test5(string filename)
    {
        if (string.IsNullOrEmpty(filename))
        {
            throw new ArgumentNullException("error");
        }
        string filepath = Path.Combine("/FILESHARE/images", filename);
        // ruleid: file-taint
        return File.ReadAllBytes(filepath);
    }

    public IActionResult OkTest4(int filename)
    {
        if (string.IsNullOrEmpty(filename))
        {
            throw new ArgumentNullException("error");
        }
        string filepath = Path.Combine("/FILESHARE/images", filename);
        // ok: file-taint
        return File.ReadAllBytes(filepath);
    }
}

[NonController]
public class NotController
{
    public IActionResult OkTest5(string filename)
    {
        if (string.IsNullOrEmpty(filename))
        {
            throw new ArgumentNullException("error");
        }
        string filepath = Path.Combine("/FILESHARE/images", filename);
        // ok: file-taint
        return File.ReadAllBytes(filepath);
    }

}
