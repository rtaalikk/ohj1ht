using System;
using System.Collections.Generic;
using System.IO.Pipes;
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
    PhysicsObject olio;

    PhysicsObject alaReuna;

    Image pelaajanKuva = LoadImage("cartoon-cat-6761858_1280.png");

    Vector nopeusYlos = new Vector(0, 900);
    Vector nopeusVasen = new Vector(-300, 0);
    Vector nopeusOikea = new Vector(300, 0);

    IntMeter pistelaskuri;

    
    /// <summary>
    /// kutsutaan peliin tarvittavat aliohjelmat
    /// </summary>
    public override void Begin()
    {
        Luokentta();
        LisaaNappaimet();
        PisteLaskuri();
        
    }

    
    /// <summary>
    /// Luodaan Luokenttä aliohjelma jonka avulla voidaan luoda kentälle hahmoja
    /// </summary>
    private void Luokentta()
    {
        int[] xsuunta = new [] { 20, -20, 40, -60, 70, -80, -100, 90, 22, -55, 100, -100, 20, -150 };
        int[] ysuunta = new[] {-200, 10, 220, 450, 610, 880, 1020, 1260, 1430, 1610, 1890, 2020, 2230, 2440};
        int[] rahay = new[] {-100 ,50 , 270, 490, 660, 920, 1100, 1290, 1500, 1650, 1990, 2100, 2300, 2470};
        int[] rahax = new[] {30, -33, -50, 40, 60, -40, 20, -20, -80, -110, 70, 20, -55, -100, -200, 20, 200, 100};
        
        olio = LuoPelaaja(0.0, -190.0);
        
        Gravity = new Vector(0, -981.0);
        
        for(int i = 0; i < xsuunta.Length; i++)
        {
           LuoLiuska(xsuunta[i], ysuunta[i]);
            
           LuoRaha(rahax[i], rahay[i]);
        }
        
        alaReuna = Level.CreateBottomBorder();
        alaReuna.IsVisible = false;
        Level.Background.Color = Color.Cyan;
        Camera.Follow(olio);
    }
    
    
/// <summary>
/// Aliohjelma, joka tekee liuskan pelikentälle
/// </summary>
/// <param name="x"> Liuskan x arvo</param>
/// <param name="y">Liuskan y arvo</param>
    private void LuoLiuska(double x, double y)
    {
        PhysicsObject liuska = PhysicsObject.CreateStaticObject(150.0, 10);
        
        liuska.Shape = Shape.Rectangle;
        liuska.X = x;
        liuska.Y = y;
        liuska.Color = Color.Black;
        
        Add(liuska);
    }
    
    
/// <summary>
/// Näpppäinten lisääminen
/// </summary>
    private void LisaaNappaimet()
    {
        Keyboard.Listen(Key.Left, ButtonState.Pressed, Liikuta, "Pelaaja liikkuu vasemmalle", olio, nopeusVasen);
        Keyboard.Listen(Key.Right, ButtonState.Pressed, Liikuta, "Pelaaja liikkuu oikealle", olio, nopeusOikea);
        Keyboard.Listen(Key.Up, ButtonState.Pressed, Liikuta, "Pelaaja liikkuu ylös", olio, nopeusYlos);
        
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }

    
/// <summary>
/// Aliohjelma, jossa luodaan pelaaja
/// </summary>
/// <param name="x">Olion x kordinaatti</param>
/// <param name="y">Olion y kordinaatti</param>
/// <returns>Palauttaa olion</returns>
    private PhysicsObject LuoPelaaja(double x, double y)
    {
        olio = new PhysicsObject(40, 40);
        
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
/// luodaan valuutta pelikentälle
/// </summary>
/// <param name="x">rahan x kordinaatti</param>
/// <param name="y">rahan y kordinaatti</param>
    private void LuoRaha(double x, double y)
    {
        PhysicsObject raha = PhysicsObject.CreateStaticObject(40.0, 40.0);
        
        raha.IgnoresCollisionResponse = true;
        raha.X = x;
        raha.Y = y;
        raha.Color = Color.Yellow;
        raha.Shape = Shape.Diamond;
        raha.Tag = "raha";
        
        Add(raha);
    }


/// <summary>
/// Tehdään liikkuminen mahdolliseksi
/// </summary>
    private void Liikuta(PhysicsObject olio, Vector nopeus)
    {
        olio.Move(nopeus);
    }


/// <summary>
/// Rahan tormays luodaan
/// </summary>
/// <param name="olio">Pelin olio</param>
/// <param name="raha">Pelin valuutta</param>
    private void Tormays(PhysicsObject olio, PhysicsObject raha)
    {
        MessageDisplay.Add("Sait rahaa!");
        raha.Destroy();
        if (olio != raha) pistelaskuri.Value += 1;
    }


/// <summary>
/// pistelaskurin luominen
/// </summary>
    private void PisteLaskuri()
    {
        pistelaskuri = new IntMeter(0);
        
        Label pistenaytto = new Label();
        pistenaytto.X = Screen.Left + 100;
        pistenaytto.Y = Screen.Top - 100;
        pistenaytto.Title = "Rahaa: ";
        pistenaytto.BindTo(pistelaskuri);
        Add(pistenaytto);
        
        pistelaskuri.MaxValue = 10;
        pistelaskuri.UpperLimit += AloitaAlusta;
    }


    /// <summary>
    /// Aloitetaan peli alusta
    /// </summary>
    private void AloitaAlusta()
    {
        ClearAll();
        Luokentta();
        LisaaNappaimet();
        PisteLaskuri();
    }

    
}