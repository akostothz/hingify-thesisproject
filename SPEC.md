# Kliensoldali fejlesztés féléves feladat

<br>

> Tóth Ákos Zalán, NK: UNN1N9

<br>

## *Feladatleírás*

Az elkészített rendszer zenéket ajánl *nap és napszakra[^1] lebontva* a felhasználó *zenehallgatási előzményeiből és a hallgatási szokásaiból*.
A zeneajánlás egy egyszerű heurisztikus módszerrel történik, euklideszi távolság segítségével, a zeneszámok pár fontosabb tulajdonsága alapján.
A felhasználónak emellett lehetősége van pontosabb keresésre is több paraméterrel, egy zeneszámhoz a legközelebbi értékű zenék megtalálására is.
A felhasználó emellett szert tehet a demográfiai adatiból levonódó lejátszási listák megtalálására is, mint például ugyanabból az országból leghallgatottabb zenék
megtalálására, vagy a felhasználó neme alapján megtalálni, hogy mások mit kedveltek az adott nemben, stb.
A felhasználók emellett statisztikai adatok megtekintésére is lehetőséget kapnak gamifikáció gyanánt, mely egy külön fülön elérhető, az adott nap[^2], 
hét, hónap és évre lebontva.

<br>
<br>

## *Funció / végpont lista*

- ***Account Controller***
     - ```api/account/register```: a felhasználók az alap adatok és demográfiai adatok megadása után tudnak regisztrálni. 
     - ```api/account/login```: a felhasználók a felhasználónevüket és jelszavukat megadva beléphetnek az oldalra.
     - ```api/account/updateuser```: a felhasználók bizonyos adataikat frissíthetik.
     - ```api/account/userpic/{id}```: a felhasználók képének lekérése miatt van, hogy esetleges átállításkor frissüljön.
     - ```api/account/spotifypic```: a felhasználók a Spotify-ra való autorizáció után lekérhetik és feltölthetik onnan a profilképüket.
     - ```api/account/addphoto```: a felhasználók a gépről feltölthetnek fényképet a megfelelő formátumú kép kiválasztása után.

- ***Music Controller***
     - ```api/music/getpersonalizedmix/{id}```: a felhasználók az adott nap és napszakra való mixüket kapják meg itt.
     - ```api/music/getlikedsongs/{id}```: a felhasználók a kedvelt dalaikat kapják meg itt.
     - ```api/music/getmusicbysex/{id}```: a felhasználók a megadott nem alapján kaphatják meg a leghallgatottabb dalokat.
     - ```api/music/getmusicbycountry/{id}```: a felhasználók a megadott ország alapján kaphatják meg a leghallgatottabb dalokat.
     - ```api/music/getmusicbyagegroup/{id}```: a felhasználók a megadott kor alapján kaphatják meg a leghallgatottabb dalokat az adott korcsoportban[^3].
     - ```api/music/search```: a felhasználók kereshetnek zeneszámokat az adatbázisban.
     - ```api/music/addsongwithlistening```: a felhasználók hozzáadhatnak zeneszámokat az adatbázisba egy Spotify API hívás segítségével, ha esetleg még nem szerepel benne.
     - ```api/music/addsongwithcid```: a felhasználók hozzáadhatnak zeneszámokat az adatbázisba Spotify ID segítségével, ha esetleg még nem szerepel benne.
     - ```api/music/findmore/{id}```: a felhasználók az adott zeneszámhoz megkaphatják pontosabb kereséssel a legközelebbi dalokat.
     - ```api/music/getstatistics```: a felhasználók a különböző zenehallgatási statisztikáit nézheti meg itt.
     - ```api/music/likesong```: a felhasználók kedvelhetnek dalokat az oldalon.
     - ```api/music/listen```: a felhasználók Spotify API segítségével elindíthatnak különböző zenéket.

- ***Error Controller***
     - ```error/not-found```: az esetleges rossz oldalra való routing hiba kiírása érdekében van.
     - ```error/server-error```: az esetleges backend hiba kiírása érdekében van.
     - ```error/bad-request```: az esetleges bad request hiba kiírása érdekében van.

<br>
<br>

## *Technológiák / keretrendszerek*

- ***Keretrendszerek***

     - Az alkalmazás ASP.NET 6 C#, illetve Angular 15 segítségével készül.
     - Frontend területén még Bootstrap 5, illeve Bootswatch segítségével készül a stílusozás
     - Az alkalmazés a felhasználói élmény növelése érdekében különböző külső könyvtárakat is felhasznál (pl. NgxSpinner, Toastr)

- ***Külső API-k***

     - <ins>Spotify API</ins>: zenék hallgatására, adatok lekérdezése, és hasonló dolgokra van. A felhasználónak rendelkeznie kell prémium előfizetéssel néhány funkció eléréséhez.
     - <ins>Cloudinary API</ins>: a felhasználók képeinek eltárolására és transzformálása miatt van rá szükség.

<br>
<br>

## *Tervezett kinézet*

Itt még csak a fontosabb komponensek látványképeit taglalom.

- ***For You***
     <br><br>
     ![For You](https://res.cloudinary.com/dt8loqugk/image/upload/v1683049035/spec-pics/foryou_pnji1x.png)
     <br>
     
- ***My Playlists***
     <br><br>
    ![My playlists](https://res.cloudinary.com/dt8loqugk/image/upload/v1683048571/spec-pics/myplaylists_1_n6wifb.png)
     <br>
     
- ***Search***
     <br><br>
    ![Search](https://res.cloudinary.com/dt8loqugk/image/upload/v1683047698/spec-pics/searchfrfr_tuj6nl.png)
     <br>
     
- ***Statistics***
     <br><br>
    ![Statistics](https://res.cloudinary.com/dt8loqugk/image/upload/v1683049792/spec-pics/statss_j5ofzn.png)
     <br>


<br>
<br>

[^1]: Napszakok: Hajnal (0h-5h), Reggel (5h-9h), Délelőtt (9h-12h), Délután (12h-17h), Este (17h-21h), Éjszaka (21h-0h)
[^2]: Nap: Az adott nap nem csak a jelenlegi napra értetődik, hanem a jelenlegi évben az összes azon nevű napjára.
[^3]: Korcsoportok: 1-11, 12-17, 18-25, 26-39, 40-59, 60+
