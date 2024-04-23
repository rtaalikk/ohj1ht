using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;

namespace Tasohyppelypeli;

/// @author Roni Taalikka
/// @version 22.04.2024
/// <summary>
/// Tehdään Tasohyppelypeli
/// </summary>
public class Tasohyppely : PhysicsGame
{
    private PhysicsObject liuska;

    private PhysicsObject olio;

    private PhysicsObject raha;

    private PhysicsObject alaReuna;

    private Image pelaajanKuva = LoadImage("cartoon-cat-6761858_1280.png");

    private Vector nopeusYlos = new Vector(0, 700);
    private Vector nopeusVasen = new Vector(-100, 0);
    private Vector nopeusOikea = new Vector(100, 0);

    
    public override void Begin()
    {
        Luokentta();
        LisaaNappaimet();
        
    }

    
    /// <summary>
    /// Luodaan Luokenttä aliohjelma jonka avulla voidaan luoda kentälle hahmoja
    /// </summary>
    void Luokentta()
    {
        int[] xsuunta = new [] { 20, -20, 40, -60, 70, -80, -100, 90, 22, -55, 100, -100, 20, -150 };
        int[] ysuunta = new[] {-200, 0, 200, 400, 600, 800, 1000, 1200, 1400, 1600, 1800, 2000, 2200, 2400};
        int[] rahay = new[] {-100 ,50 , 230, 450, 660, 900, 1100, 1250, 1500, 1650, 1890, 2100, 2300, 2470};
        int[] rahax = new[] {30, -33, -50, 40, 60, -40, 20, -20, -80, -110, 70, 20, -55, -100, -200, 20, 200, 100};
        
        olio = LuoPelaaja(0.0, -190.0);
        
        Gravity = new Vector(0, -981.0);
        
        for(int i = 0; i < xsuunta.Length; i++)
        {
            liuska = LuoLiuska(xsuunta[i], ysuunta[i]);
            
            raha = LuoRaha(rahax[i], rahay[i]);
        }
        
        alaReuna = Level.CreateBottomBorder();
        alaReuna.IsVisible = false;
        Level.Background.Color = Color.Cyan;
        
        Camera.Follow(olio);
        
    }
    
    
/// <summary>
/// Tehdään liuska jonka avuilla pelaaja hyppii
/// </summary>
/// <param name="Liuska"></param>
/// <returns>Palauttaa liuskan</returns>
    PhysicsObject LuoLiuska(double x, double y)
    {
        PhysicsObject liuska = PhysicsObject.CreateStaticObject(150.0, 10);
        liuska.Shape = Shape.Rectangle;
        liuska.X = x;
        liuska.Y = y;
        liuska.Color = Color.Black;
        Add(liuska);
        return liuska;
    }
    
    
/// <summary>
/// Näpppäinten lisääminen
/// </summary>
    private void LisaaNappaimet()
    {
        Keyboard.Listen(Key.Left, ButtonState.Pressed, Liikuta, "Pelaaja liikkuu vasemmalle", olio, nopeusVasen);
        Keyboard.Listen(Key.Right, ButtonState.Pressed, Liikuta, "Pelaaja liikkuu oikealle", olio, nopeusOikea);
        Keyboard.Listen(Key.Up, ButtonState.Pressed, Hyppy, "Pelaaja liikkuu ylös", olio, nopeusYlos);
        
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }

    
/// <summary>
/// Luodaan pelaaja, jota pystyy liikuttamaan
/// </summary>
/// <param name="Pelaaja"></param>
/// <returns>Palauttaa olion</returns>
    PhysicsObject LuoPelaaja(double x, double y)
    {
        PhysicsObject olio = new PhysicsObject(40, 40);
        
        olio.Shape = Shape.Circle;
        olio.Color = Color.Red;
        olio.Mass = 1.0;
        olio.Image = pelaajanKuva;
        
        olio.X = x;
        olio.Y = y;
        
        AddCollisionHandler(olio, "raha", Tormays);
        
        Add(olio);
        return olio;
    }


/// <summary>
/// Luodaan raha, jota pelaajan tulee kerät
/// </summary>
/// <param name="Raha"></param>
/// <returns>Rahan</returns>
    PhysicsObject LuoRaha(double x, double y)
    {
        PhysicsObject raha = PhysicsObject.CreateStaticObject(40, 40);
        
        raha.IgnoresCollisionResponse = true;
        
        raha.X = x;
        raha.Y = y;
        
        raha.Color = Color.Yellow;
        raha.Shape = Shape.Diamond;
        raha.Tag = "raha";
        
        Add(raha);
        return raha;
    }


/// <summary>
/// Tehdään sivuttaissuunnnan liike
/// </summary>
    void Liikuta(PhysicsObject olio, Vector nopeus)
    {
        olio.Move(nopeus);
    }


/// <summary>
/// Tehdään hyppäämisliike
/// </summary>
    void Hyppy(PhysicsObject olio, Vector nopeus)
    {
        olio.Hit(nopeus);
    }


/// <summary>
/// Rahan tormays luodaan
/// </summary>
/// <param name="olio"></param>
/// <param name="raha"></param>
    private void Tormays(PhysicsObject olio, PhysicsObject raha)
    {
        MessageDisplay.Add("Sait rahaa!");
        raha.Destroy();
        
        if (alaReuna == olio)
        {
            AloitaAlusta();
        }
    }


/// <summary>
/// Aloitetaan peli alusta
/// </summary>
    private void AloitaAlusta()
    {
        ClearAll();
        Luokentta();
        LisaaNappaimet();
    }

}