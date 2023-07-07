# C City

## Bevezetés

A C City egy egyjátékos, valós idejű város építő szimulátor, ahol a játékos felépítheti a saját városát és menedzselheti polgármesterként. A játék célja, hogy a játékos egy virágzó várost hozzon létre, aminek a költségvetése kiegyensúlyozott és boldogok a polgárok.

## Mit tud tenni a játékos?

- Építhet:
  - Új lakó- , ipari- vagy szolgáltatás zónát
  - Összekötő utakat
  - Villany vezetéket
  - Kiszolgáló épületeket
- Megtekintheti:
  - Egyes zónák statisztikáját
  - A város statisztikáját
- Az adó mértékén állíthat
- Zónákat fejleszthet

## Miket tud építeni a játékos?

- Lakó-, ipari- és szolgáltatás zónákat
- Összekötő utakat
- Villany vezetéket
- Rendőrséget
- Tűzoltóságot
- Erőművet
- Stadiont
- Erdőket

Az egyes épületek különböző hatással vannak a városra, illetve közvetlen környezetükre. Pl. a stadion megnöveli a körülötte lakó/dolgozó polgárok elégedettségét, a rendőrség épülete pedig a biztonságérzetet növeli. Ezek mind az elégedettség javításához vezetnek. Természetesen egy épület negatív hatással is lehet a körülötte lévő mezőkre. Ilyen pl. az ipari zóna. Ezen hatásokat erdők építésével lehet redukálni, leárnyékolni. Egy épület (az erdőt leszámítva természetesen) **csak akkor fejti ki hatását, ha közútról elérhető.**

## A polgárok

Minden hónap elején a város mutatóitól függően (elégedettség, adósság, stb.) polgárok költöznek be a városba. Minden polgár a lehető legközelebbi munkahelyen vállal munkát. Fontos, hogy a polgárok **csak olyan zónába költöznek be / vállalnak munkát**, amely **áramellátás alatt van, illetve közútról elérhető**. A város minden polgára rendelkezik egy elégedettségi mutatóval, amelyet pozitívan befolyásolnak a következő tényezők:
- alacsony adók;
- lakóhelyhez közeli munkahely;
- lakóhelyhez nincsen közel ipari épület;
- lakóhely és munkahely közbiztonsága (amely a népesség növekedésével arányosan egyre súlyosabb szempont).

Negatívan befolyásolják az elégedettséget a következő tényezők:

- a fenti pozitív tényezők ellentétei;
- ha a város negatív büdzsével rendelkezik (hitelből működik), ez a faktor legyen arányos azzal,
hogy mekkora hitelről van szó és hány éve negatív a büdzsé;
- ha kiegyensúlyozatlan a városban a szolgáltatások és az ipari termelés aránya.

**Amennyiben a teljes város elégedettsége kritikusan alacsonnyá válik, leváltják a polgármestert és a játékos vesztett**


## Közút és áramellátás

A játék kezdetekor, a pálya alján egy épített út mező található. A játék folyamán **azok az utak minősülnek közútnak, amelyek elérhetőek a kezdő út mezőről.**. A kezdő út mező kivételével az utak elbonthatók abban az esetben, amennyiben egy korábban már felépített épület a rombolás hatására nem válna közútról elérhetetlenné.

A városban minden épületnek áramellátásra van szüksége. **Az erőmű a vele szomszédos mezőket látja el árammal**, amennyiben azok zóna mezők vagy kiszolgáló épületek. Minden zóna mező és kiszolgáló épület automatikusan rendelkezik épített elektromos hálózattal az áramszolgáltatás továbbítására, így az **tovább terjed az összefüggő zónákból és kiszolgáló épületekből álló területen**. Nem összefüggő területek között **magasfeszültségű távvezeték építésével továbbítható az áramszolgáltatás**, ezen mezőkre más nem építhető ez esetben. Az utcákon (1 egységnyi úton) is automatikusan tovább terjed az áramellátás.

## Képek
![Főmenü](Screenshots/fomenu.png)
![1](Screenshots/1.png)
![2](Screenshots/2.png)
![3](Screenshots/3.png)
![4](Screenshots/4.png)
![5](Screenshots/5.png)

---

## Asset attribution

### Game icon

|||||
|-|-|-|-|
|Downloaded from [iconduck.com](https://iconduck.com/emojis/133731/shark). It's part of the emoji set [Microsoft Fluent UI Emoji Set](https://iconduck.com/sets/microsoft-fluentui-emoji-set).|![shark](uploads/a1462bc6972e493bdfa932e747838669/shark.png)|Downloaded from [iconduck.com](https://iconduck.com/emojis/133731/shark). It's part of the illustration set [ManyPixels Illustrations](https://iconduck.com/sets/manypixels-illustrations).|![new-york-city](uploads/2dd558eea8ea91b665c8ba019f89adae/new-york-city.png)|

### Tool icons

| | | | | |
|-|-|-|-|-|
|<a href="https://www.flaticon.com/free-icons/police" title="police icons">Police icons created by Trazobanana - Flaticon</a>|![tool1](uploads/c3af351841d2a350a849786084681a98/tool1.png)| |<a href="https://www.flaticon.com/free-icons/firefighter" title="firefighter icons">Firefighter icons created by Futuer - Flaticon</a>|![tool2](uploads/b6a47042a0e87a9d1a3c687b013ec14b/tool2.png)|
|<a href="https://www.flaticon.com/free-icons/electric-pole" title="electric pole icons">Electric pole icons created by Freepik - Flaticon</a>|![tool3](uploads/31889b88968a5c1ec2372a89f5513133/tool3.png)| |<a href="https://www.flaticon.com/free-icons/bulldozer" title="bulldozer icons">Bulldozer icons created by Freepik - Flaticon</a>|![tool4](uploads/f761dd5969168f03a828564293d766a8/tool4.png)
|<a href="https://www.flaticon.com/free-icons/tree" title="tree icons">Tree icons created by Freepik - Flaticon</a>|![tool5](uploads/c06ab1dc1e39ce7644338b36e3624120/tool5.png)| |<a href="https://www.flaticon.com/free-icons/soccer" title="soccer icons">Soccer icons created by Smashicons - Flaticon</a>|![tool6](uploads/6ca18451765dc3af027793388a2f75bc/tool6.png)|
|<a href="https://www.flaticon.com/free-icons/thunder" title="thunder icons">Thunder icons created by Smashicons - Flaticon</a>|![tool7](uploads/bae6cc71e90faa91659d8b5752d90f13/tool7.png)| |<a href="https://www.flaticon.com/free-icons/cursor" title="cursor icons">Cursor icons created by Freepik - Flaticon</a>|![tool8](uploads/ac159e45542d7a75a80487d28ca31c06/tool8.png)|
|<a href="https://www.flaticon.com/free-icons/home" title="home icons">Home icons created by Vectors Market - Flaticon</a>|![tool9](uploads/06be437189dcae439bd69fc53c8e3b10/tool9.png)| |<a href="https://www.flaticon.com/free-icons/shop" title="shop icons">Shop icons created by Freepik - Flaticon</a>|![tool10](uploads/df74509142f7c3959e8c2d46efc12433/tool10.png)|
|<a href="https://www.flaticon.com/free-icons/factory" title="factory icons">Factory icons created by Smashicons - Flaticon</a>|![tool11](uploads/e36e29523c3d5d51b361fc40ecf7363e/tool11.png)|

### Textures

| | | |
|-|-|-|
|fireDepartment<br />fireDepartment_1_0<br />policeDepartment<br />policeDepartment_1_0<br />Zones/commercialZoneIntermediateHalf<br />Zones/commercialZoneIntermediateFull|<a href="https://www.freepik.com/free-vector/urban-buildings-icon-set_4029334.htm#query=police%20station&position=18&from_view=search&track=ais">Image by macrovector</a> on Freepik|![texture1](uploads/8d1409dde1d37fb68e3714df8691549e/texture1.jpg)|
|Zones/residentialZoneAdvancedHalf<br />Zones/residentialZoneAdvancedFull|<a href="https://www.freepik.com/free-vector/skyscraper-offices-set_1531733.htm#query=skyscraper&position=9&from_view=search&track=sph">Image by macrovector</a> on Freepik|![texture2](uploads/2aa301a17302abcf94e9b98ddf352ecc/texture2.jpg)|
|Zones/commercialZoneAdvancedHalf<br />Zones/commercialZoneAdvancedFull|<a href="https://www.freepik.com/free-vector/shopping-mall-icons-set_4266203.htm">Image by macrovector</a> on Freepik|![texture3](uploads/4101ac329ca6b3c00c23485c3c66f698/texture3.jpg)|
|Zones/residentialZoneBeginnerHalf<br />Zones/residentialZoneBeginnerFull<br />Zones/residentialZoneIntermediateHalf<br />Zones/residentialZoneIntermediateFull|<a href="https://www.freepik.com/free-vector/residential-house-buildings_1531715.htm">Image by macrovector</a> on Freepik|![texture4](uploads/b1b2de93526602f7a28f9262481f4841/texture4.jpg)|
|Zones/powerPlant<br />Zones/powerPlant_0_1<br >Zones/powerPlant_1_0<br >Zones/powerPlant_1_1|<a href="https://www.freepik.com/free-vector/industrial-buildings-flat-set-petroleum-industry-power-plants-power-stations-oil-offshore-platform-isolated-illustration_6869907.htm">Image by macrovector</a> on Freepik|![texture5](uploads/83cdedb36b4d2461ccbbbf7b731e1952/texture5.jpg)|
|Zones/industrialZoneAdvancedHalf<br />Zones/industrialZoneAdvancedFull|<a href="https://www.freepik.com/free-vector/factory-decorative-flat-icons-set_9399463.htm">Image by macrovector</a> on Freepik|![texture6](uploads/35027ebb7a4b240b95fb2655628bf9dd/texture6.jpg)|
|Zones/industrialZoneIntermediateHalf<br />Zones/industrialZoneIntermediateFull|<a href="https://www.freepik.com/free-vector/industrial-building-set_3815780.htm">Image by macrovector</a> on Freepik|![texture7](uploads/7a43d0b2bdb44419430116590a9e0b09/texture7.jpg)|
|Zones/industrialZoneBeginnerHalf<br />Zones/industrialZoneBeginnerFull|<a href="https://www.freepik.com/free-vector/industrial-buildings-flat_1531708.htm#query=factory&position=0&from_view=author">Image by macrovector</a> on Freepik|![texture8](uploads/e5c5d60d552adfa23501564dff0fef18/texture8.jpg)|
|stadium<br />stadium_0_1<br />stadium_1_0<br />stadium_1_1|<a href="https://www.freepik.com/free-vector/front-view-sport-stadiums-flat-set_12291013.htm">Image by pch.vector</a> on Freepik|![texture9](uploads/fac3e6030de19eb8bbef4cf7d818c675/texture9.jpg)|
|forest<br />forestHalf<br />forestFull<br />stadium<br />stadiun_1_0|Image by <a href="https://www.freepik.com/free-vector/flat-design-type-trees-set_18895282.htm#query=tree&position=45&from_view=search&track=sph">Freepik</a>|![texture10](uploads/a6dac4acb6c0348305a37c5ed8f29f7b/texture10.jpg)|
|Zones/commercialZoneBeginnerHalf<br />Zones/commercialZoneBeginnerFull|<a href="https://www.freepik.com/free-vector/cafe-storefront-flat-set_4331387.htm#query=shop%20facade&position=0&from_view=keyword&track=ais">Image by macrovector</a> on Freepik|![texture11](uploads/194ff5cdea00538960d0d6b417e60c2a/texture11.jpg)|
|background|<a href="https://www.freepik.com/free-vector/earth-texture_997013.htm#query=land%20texture&position=22&from_view=keyword&track=ais">Image by 0melapics</a> on Freepik|![texture12](uploads/288d35e34922d9b08ec6bbda00676dd8/texture12.jpg)|
|fire<br />Tools/flintAndSteel|Image by <a href="https://www.freepik.com/free-vector/assortment-flames-flat-design_1071755.htm#query=fire&position=4&from_view=search&track=sph">Freepik</a>|![texture13](uploads/3fda2459354c9c60d2fd96c2944e184c/texture13.jpg)|
|firetruck|Image by <a href="https://www.freepik.com/free-vector/flat-design-fire-station_19824419.htm#query=fire%20station&position=21&from_view=search&track=ais">Freepik</a>|![texture14](uploads/266eddaa2ebbb5c28a81f6aab99be427/texture14.jpg)|
|pole|<a href="https://www.freepik.com/free-vector/utility-pole-isolated-white-background_39653707.htm#query=electrical%20pole&position=2&from_view=search&track=ais">Image by brgfx</a> on Freepik|![texture15](uploads/9735a2f73e45f3348d0a55b93ed2e6dd/texture15.jpg)|
