using System.ComponentModel;
using StarterKit.Models;
using StarterKit.Utils;
using StarterKit.Controllers;
using Microsoft.Data.Sqlite;

namespace StarterKit.Services;

public class TheatreService : ITheatreService{
    private readonly DatabaseContext _context;

    public TheatreService(DatabaseContext context){
        _context = context;
    }
}