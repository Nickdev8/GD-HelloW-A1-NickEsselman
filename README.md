# README.md

## Project: City Drift Game

### Beschrijving
Deze Unity-game stelt de speler in staat om door een stad te rijden en te driften. De stad is ontworpen als een oneindige omgeving, waarbij gebouwen en wegen steeds opnieuw gegenereerd worden naarmate je verder rijdt. Het doel van de game is om zo lang mogelijk te rijden en je drifts te perfectioneren.

### Oplossingen en Keuzes

1. **Oneindige Stadsgeneratie**
   - **Oplossing**: Ik heb een procedural generation systeem gebruikt voor de stad, waardoor het lijkt alsof de wereld oneindig doorgaat. Hierbij worden secties van de stad dynamisch geladen en weer verwijderd als ze uit beeld zijn.
   - **Waarom**: Dit bespaart geheugen en voorkomt dat de speler door een statische, beperkte wereld rijdt. Zo behoudt de game een vloeiende ervaring, zelfs met beperkte hardware.

2. **Voertuigfysica en Driftmechaniek**
   - **Oplossing**: De auto gebruikt een aangepaste physics-controller om realistisch driften mogelijk te maken. Ik heb de wrijving en het stuurgedrag aangepast om een balans te vinden tussen uitdaging en speelplezier.
   - **Waarom**: Driften is een centraal onderdeel van de gameplay, dus het was belangrijk dat dit goed aanvoelde. De gekozen physics zorgen voor een vloeiende besturing zonder te realistisch of te moeilijk te worden.

3. **Performance Optimalisatie**
   - **Oplossing**: Object pooling is toegepast om hergebruik van gebouwen en objecten in de stad te optimaliseren. Dit zorgt ervoor dat de game ook op lagere specificaties soepel blijft draaien.
   - **Waarom**: Zonder object pooling zou de game snel vertragen door de constante creatie van nieuwe objecten. Dit was nodig om de oneindige wereld mogelijk te maken zonder prestatieproblemen.

### Reflectie en Verbeteringen

1. **Gebouwen Variatie**
   - **Volgende keer**: Ik zou meer variatie toevoegen aan de gebouwen en stadsobjecten, zodat de stad visueel interessanter wordt. De huidige oplossing kan op den duur repetitief aanvoelen.
   - **Waarom**: Variatie in de stad verhoogt de immersie en houdt de speler langer geboeid.

2. **Betere Drift Feedback**
   - **Volgende keer**: De drift-feedback kan verbeterd worden door visuele effecten, geluid en een puntenmechaniek toe te voegen wanneer je een succesvolle drift uitvoert.
   - **Waarom**: Dit zou de beloning voor driften tastbaarder maken en de game spannender.

3. **UI en Progressie**
   - **Volgende keer**: Het toevoegen van een scorebord, challenges of een progressiesysteem zou de herspeelbaarheid vergroten.
   - **Waarom**: Op dit moment ontbreekt er een duidelijke manier voor spelers om hun prestaties te meten, wat een gemiste kans is voor competitieve elementen in de game.

---

### Vereisten
- Unity versie 6 lts of hoger
- Systeem met minimaal 4GB RAM
