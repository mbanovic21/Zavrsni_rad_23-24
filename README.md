# Preschool Management Software

## Osobni podaci

Ime i prezime | E-mail adresa (FOI) | JMBAG | Github korisničko ime | Broj telefona
------------  | ------------------- | ----- | --------------------- | -------------
Matej Banović | mbanovic21@student.foi.hr | 0016154542 | mbanovic21 | +385 99 236 1810

## Opis domene
Domena razvoja softvera za praćenje rada dječjeg vrtića obuhvaća sve ključne aspekte organizacije i upravljanja dječjim vrtićem. Ovaj softver ima za cilj olakšati praćenje prisutnosti djece, izradu rasporeda aktivnosti, administraciju, evidenciju zaposlenika i komunikaciju s roditeljima. Administratorski aspekt uključuje praćenje i evidentiranje podataka o djeci, upis te praćenje i evidentiranje zaposlenika. Evidencija i praćenje djece obuhvaća ocjenjivanje napretka, generiranje izvještaja o razvoju, medicinske podatke i alergije. Također, softver omogućuje dvosmjernu komunikaciju između vrtića i roditelja putem e-pošte, dijeleći informacije o djetetovom napretku, događanjima i posebnim zahtjevima. Kroz siguran pristup podacima i definiranje pristupa različitim korisničkim ulogama, osigurava se povjerljivost i integritet informacija. Integracijom s online bazom podataka putem ADO.NET-a i .NET Framework-a omogućava se pouzdana pohrana i upravljanje podacima, čime se olakšava svakodnevno upravljanje i očuvanje povijesti o dječjem vrtiću. Sve navedene komponente zajedno čine važnu domenu za razvoj softverskog rješenja koje će unaprijediti kvalitetu pružanja usluga i zadovoljstvo svih dionika, uključujući djecu, roditelje i zaposlenike vrtića.

## Specifikacija projekta
Oznaka | Naziv | Kratki opis
------ | ----- | ----------- 
F01 | Prijavljivanje i registriranje zaposlenika u sustav | Prijavljivanje i registriranje u sustavu dječjeg vrtića ključne su funkcionalnosti koje osiguravaju siguran pristup softverskom rješenju za praćenje rada vrtića. Ova funkcionalnost omogućuje korisnicima, uključujući administratore i zaposlenike, da unesu svoje pristupne podatke dodijeljene prilikom zapošljavanja. <br> Administratori imaju poseban proces prve prijave u sustav kako bi osigurali sigurnost i jednostavnost stvaranja vlastitog korisničkog računa. Prilikom prve prijave, koriste privremene pristupne podatke: korisničko ime "admin" i lozinku "1234". Ovi podaci su privremeni, hardkodirani i koriste se isključivo za inicijalnu prijavu kako bi omogućili stvaranje vlastitog korisničkog računa. Nakon toga, administratori mogu promijeniti svoje pristupne podatke putem funkcionalnosti "Administriranje zaposlenika". <br> Uobičajeni pristupni podaci uključuju korisničko ime i lozinku, a sustav osigurava temeljitu verifikaciju kako bi zaštitio sigurnost i spriječio neovlaštene pristupe. Nakon uspješne prijave, korisnici dobivaju pristup svojim osobnim podacima i funkcionalnostima prilagođenima njihovoj ulozi u vrtiću. Ova sveobuhvatna funkcionalnost omogućuje povjerljivost podataka i pruža personalizirano iskustvo, čime se osigurava siguran i intuitivan pristup softveru za praćenje rada dječjeg vrtića.
F02 | Upisivanje djece | Funkcionalnost upisa djece omogućuje administratorima jednostavno i sigurno upisivanje djece u vrtić, pružajući podatke o djeci, kontakt informacije, medicinske informacije i druge relevantne podatke. Tijekom procesa upisa, sustav automatski dodjeljuje svakom djetetu jedinstveni identifikacijski broj i pohranjuje te informacije u online bazu podataka. Ova funkcionalnost također omogućava pregled i ažuriranje podataka o upisanoj djeci te osigurava dostupnost tih informacija kako bi ih osoblje vrtića moglo učinkovito upravljati. Uz to, pruža siguran i praktičan način za roditelje da prate status upisa svoje djece i prate komunikaciju s vrtićem tijekom tog procesa na način da će biti obaviješteni putem e-pošte.
F03 | Administriranje zaposlenika i djece | Ova ključna funkcionalnost omogućuje administratorima potpunu kontrolu nad korisnicima u sustavu, uključujući zaposlenike i upisanu djecu u vrtiću. Administratori imaju ovlasti za stvaranje, uređivanje i brisanje korisničkih računa zaposlenika i djece, pridruživanje osoblju vrtića, te definiranje njihovih prava pristupa. <br> Osim toga, ova funkcionalnost omogućuje administratorima pregled cijelog "dosjea" zaposlenika, pružajući informacije poput povijesti zaposlenika u vrtiću, u kojoj godini su radili, u kojoj vrtićkoj skupini su bili, te druge relevantne informacije. Također, omogućuje precizno upravljanje pravima pristupa za sve korisnike, praćenje prisutnosti i aktivnosti zaposlenika, te brigu o točnosti i ažuriranosti podataka o djeci. <br> Ova sveobuhvatna funkcionalnost pridonosi sigurnosti i transparentnosti rada vrtića te postaje ključnim alatom za organizaciju i upravljanje radom dječjeg vrtića. Omogućuje cjelovit pregled i kontrolu nad djecom i zaposlenicima, čime se osigurava učinkovito upravljanje korisnicima u sustavu.
F04 | Kreiranje i pregled raspored za zaposlenike | Ova sveobuhvatna funkcionalnost sustava omogućuje administratorima i zaposlenicima detaljno planiranje, kreiranje i pregledavanje rasporeda rada u dječjem vrtiću. <br> Administratorima pruža alat za izradu specifičnih radnih smjena i termina za svakog zaposlenika uz uzimanje u obzir individualnih preferencija i dostupnosti. Također, omogućuje automatsko generiranje rasporeda na temelju definiranih parametara, pojednostavljujući proces planiranja i organizacije rada osoblja. Zaposlenici imaju pristup vlastitim rasporedima putem sučelja kako bi bili informirani o svojim obavezama i aktivnostima unaprijed. <br> Osim što olakšava samim zaposlenicima praćenje njihovih radnih obveza, ova funkcionalnost također pridonosi efikasnom upravljanju osobljem za administratore. Omogućuje im stvaranje, uređivanje i prilagodbu rasporeda za svakog zaposlenika kako bi se postigla ravnoteža između radnih potreba i dostupnosti osoblja. To olakšava planiranje i organizaciju rada unutar vrtića, što na kraju povećava učinkovitost i kvalitetu usluga. Pregledavanje rasporeda rada putem sučelja također doprinosi zadovoljstvu zaposlenika jer im omogućuje bolje upravljanje vlastitim vremenom i obvezama.
F05 | Kreiranje raspored aktivnosti | Sustav će omogućiti administratorima i zaposlenicima stvaranje i upravljanje rasporedima svih aktivnosti i obveza unutar vrtića. Korisnici mogu definirati vremenske okvire, lokacije i odgovorne osobe za svaku aktivnost, uključujući nastavne satove, igre, obroke, i druge događaje. Ova funkcionalnost omogućava transparentnost u rasporedu aktivnosti za djecu i osoblje, pridonosi boljoj organizaciji rada, olakšava komunikaciju među zaposlenicima i roditeljima te pomaže u učinkovitom planiranju dnevnih rutina unutar vrtića.
F06 | Evidentiranje dolazaka djece | Funkcionalnost "Evidencija djece" predstavlja ključni dio softverskog rješenja za praćenje rada dječjeg vrtića. Ovaj modul omogućava administratorima i osoblju precizno praćenje i upravljanje informacijama o svakom djetetu u vrtiću. Unutar sustava, korisnici mogu unositi i pohranjivati podatke o svakom djetetu, uključujući osobne podatke, medicinske informacije, informacije o roditeljima, hitne kontakte, te specifične potrebe i zahtjeve. Također, evidencija djece omogućava praćenje dolazaka i odlazaka djece, kako bi se osigurala sigurnost i pouzdanost njihove prisutnosti. Ova funkcionalnost olakšava komunikaciju s roditeljima, osigurava brzu reakciju na hitne situacije, te doprinosi kvalitetnom pružanju skrbi i obrazovanja u dječjem vrtiću.
F07 | Kreiranje i distribucija obavijesti za roditelje i zaposlenike | Ova ključna funkcionalnost omogućuje administratorima izradu obavijesti namijenjenih roditeljima čija su djeca dio sustava, kao i zaposlenicima, uz korištenje strukturiranih obrazaca ili formulara. Obavijesti se kreiraju kako bi pružile važne informacije i distribuiraju se automatski putem e-maila. <br> Za roditelje, ovo olakšava brzu i preciznu komunikaciju s vrtićem, omogućujući dijeljenje važnih informacija o događanjima u vrtiću, roditeljskim sastancima, i drugim potrebama. Strukturirane obavijesti pomažu roditeljima da jasno razumiju informacije i odgovore na postavljena pitanja ili podnesu tražene podatke. <br> Za zaposlenike, ova funkcionalnost omogućuje brzu komunikaciju s administracijom i olakšava dijeljenje informacija o promjenama u rasporedu, specifičnim zadacima i drugim relevantnim temama. Također, strukturiranje obavijesti kao obrazaca pomaže u jasnom organiziranju informacija. <br> Obje skupine korisnika primaju obavijesti putem e-maila, osiguravajući da budu informirani i uključeni u relevantne aspekte rada dječjeg vrtića. Ova funkcionalnost unapređuje komunikaciju, suradnju i organizaciju u vrtiću, čineći proces razmjene informacija bržim i transparentnijim za sve uključene strane.
F08 | Vrednovanje djece bilješkom | Ova funkcionalnost omogućava osoblju vrtića da sustavno bilježi i prati napredak i ponašanje svakog djeteta. To uključuje unos i ažuriranje komentara i povratnih informacija vezanih uz djetetovu edukaciju i ponašanje. Osim toga, omogućava generiranje izvještaja i analiza, koje pružaju dublji uvid u napredak djece tijekom vremena. Bilješke i izvještaji koriste se za praćenje razvoja, pružaju roditeljima informacije o djetetovom napretku, te pomažu vrtiću u kontinuiranom poboljšanju programa i pristupa obrazovanju. Ova funkcionalnost osigurava sustavno praćenje i evaluaciju rada vrtića, što rezultira boljim iskustvom i razvojem djece.
F09 | Izrada izvještaja | Ova funkcionalnost omogućuje korisnicima, uključujući administratore, zaposlenike i roditelje, da generiraju i preuzmu izvještaje u formatu PDF-a. Kroz ovu funkcionalnost, korisnici mogu pregledati i generirati različite vrste izvještaja, kao što su evidencija prisutnosti, financijski izvještaji, raspored aktivnosti, ili izvještaji o individualnoj djeci. Naravno izvještaji su zaštićeni ulogom koju korisnici imaju u sustav, te tako nisu svi izvještaji svakom vidljivi. Izvještaji se automatski formatiraju u PDF dokumente, što omogućuje jednostavno pregledavanje i dijeljenje informacija. Ovaj format također osigurava da izvještaji zadrže svoj izvorni izgled i strukturu, čineći ih prikladnima za ispis ili digitalno dijeljenje. "Ispis izvještaja u obliku PDF dokumenta" pridonosi efikasnosti i organiziranosti rada vrtića, omogućujući korisnicima da brzo i jednostavno pristupe relevantnim informacijama u obliku koji je prikladan za njihove potrebe. Ova funkcionalnost također poboljšava transparentnost i komunikaciju unutar vrtića, pružajući relevantne informacije svim uključenim stranama.
F10 | Otvaranje i upravljanje vrtićkom godinom | Ova funkcionalnost omogućuje administratorima kontrolu i upravljanje vrtićkim godinama, slično konceptu školskih i akademskih godina. Proces otvaranja nove vrtićke godine postaje olakšan i organiziran. <br> Administratori imaju mogućnost stvaranja novih vrtićkih skupina za svaku vrtićku godinu, prebacivanje postojećih skupina u novu godinu te obavljanje drugih relevantnih operacija. Sustav omogućuje precizno definiranje parametara za svaku vrtićku godinu, kao što su popis vrtićkih skupina, raspored aktivnosti i povezivanje djece s odgovarajućim skupinama. <br> Ova funkcionalnost olakšava organizaciju rada u vrtiću, omogućuje praćenje napretka djece kroz godine, i pruža administraciji bolje upravljanje resursima te prilagodbu potrebama vrtića. Otvoriti i upravljati vrtićkim godinama postaje ključno za efikasno funkcioniranje vrtića i pružanje kvalitetnih usluga djeci.
F11 | Upravljanje opremom i resursima | Ova funkcionalnost omogućuje administratorima vrtića učinkovito praćenje i upravljanje svom opremom, igračkama, knjigama i drugim ključnim resursima unutar vrtića. Administracija može detaljno voditi inventar svih tih resursa kako bi se osiguralo da su dostupni kada su potrebni za obrazovne i rekreativne svrhe djece. <br> Sustav omogućuje administratorima da evidentiraju svaku stavku opreme, uključujući informacije o broju, stanju, datumu nabave te o potrebama za održavanjem. Na taj način, upravljanje potrošnim materijalima i njihova zamjena postaje transparentnija i učinkovitija, budući da sustav pruža obavijesti o potrebama za obnovom resursa kada su pri kraju ili su oštećeni. <br> Osim toga, funkcionalnost također omogućuje administraciji praćenje i analizu korištenja resursa, pomažući u planiranju budžeta za nabavku nove opreme i resursa. Kroz ovu funkcionalnost, vrtić može osigurati da su djeca opremljena kvalitetnim materijalima i resursima koji podržavaju njihovu igru, učenje i zabavu, čime se doprinosi bogatom iskustvu djece unutar vrtića.
F12 | Grafički prikaz statističkih podataka | Ova funkcionalnost omogućuje sustavu da generira i prikaže grafičke prikaze statističkih podataka povezanih s radom dječjeg vrtića. Grafički prikazi, kao što su grafikoni, dijagrame i tablice, koriste se kako bi se informacije prezentirale na vizualno privlačan način. Sustav će omogućiti korisnicima, posebice administratorima, da lako interpretiraju složene statističke podatke, identificiraju trendove, analiziraju ključne pokazatelje i donose informirane odluke. <br> Grafički prikazi mogu obuhvaćati različite aspekte rada vrtića, kao što su prisutnost djece, financijski pokazatelji, stanje resursa ili učinkovitost osoblja. <br> Ova funkcionalnost pridonosi boljem razumijevanju i praćenju ključnih informacija, olakšava donošenje odluka i pruža vizualno privlačan način prezentacije podataka. Grafički prikazi statističkih podataka čine upravljanje dječjim vrtićem učinkovitijim i transparentnijim, potičući kontinuirano unapređenje rada.

## Tehnologije i oprema
Pri implementaciji softverskog rješenja za praćenje rada dječjeg vrtića koristit ćemo niz tehnologija, alata i opreme kako bismo osigurali kvalitetu, funkcionalnost i sigurnost rješenja. Ovdje je popis tih elemenata:

* [**Programski jezik C#**](https://learn.microsoft.com/en-us/visualstudio/get-started/csharp/?view=vs-2022): Korišten za razvoj glavnog softverskog rješenja.

* [**Windows Forms .NET**](https://learn.microsoft.com/en-us/dotnet/desktop/winforms/overview/?view=netdesktop-7.0): Korišten za razvoj grafičkog korisničkog sučelja, pružajući bogat, stabilan i interaktivan dizajn.

* [**Visual Studio**](https://visualstudio.microsoft.com/): Kao integrirano razvojno okruženje za programiranje u C# i Windows Presentation Foundation (WPF) .NET.

* [**SQLite, MySQL**](https://sqlitebrowser.org/): Za pohranu podataka o djeci, osoblju, rasporedu, aktivnostima i administrativnim informacijama.

* [**ADO.NET**](https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/ado-net-overview): Za integraciju i upravljanje online bazom podataka, omogućujući siguran pristup i manipulaciju podacima.

* [**Entity Framework**](https://learn.microsoft.com/en-us/ef/): Za olakšavanje upita i pristupa podacima u aplikaciji.

* [**SMTP server**](https://support.google.com/a/answer/176600?hl=en): Za slanje e-mail obavijesti roditeljima.

* [**Operativni sustav Windows**](https://www.microsoft.com/en-us/windows?r=1): Kako bi aplikacija bila kompatibilna s okruženjem većine korisnika.

* [**Projektni alati (Gantt Chart)**](https://www.onlinegantt.com/#/gantt): Za upravljanje projektom, planiranje aktivnosti, i praćenje napretka.

* [**Version Control Platform (GitHub)**](https://github.com/): Za upravljanje izvorima koda, verzioniranje i timsku suradnju.

* [**Visual Paradigm**](https://www.visual-paradigm.com/whats-new/): Za dizajniranje i kreiranje dijagrama

* [**Dokumentacijski alati (Microsoft Office Education)**](https://www.microsoft.com/en-us/education/products/microsoft-365): Za izradu tehničke dokumentacije, uputa za korištenje i dokumentacije korisnika.

Svi korišteni alati i tehnologije su javno dostupni i imaju odgovarajuće licence kako bi se osigurala legalnost i usklađenost s pravilima i propisima. Za tehnologije koje nisu standardno dostupne (npr. Entity Framework) u dokumentaciji će biti navedeni načini instalacije i korištenja.
