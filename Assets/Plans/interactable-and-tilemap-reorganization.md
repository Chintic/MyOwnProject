# Project Overview
- **Game Title**: Bakery 2D (Top-Down Pixel Art)
- **High-Level Concept**: Um jogo 2D top-down aconchegante de culinária e gerenciamento de padaria onde o jogador coleta ingredientes, interage com bancadas e objetos, prepara itens (farinha, pães) e atende pedidos em uma estética pixel art.
- **Players**: Single-player
- **Inspiration / Reference Games**: Overcooked, Stardew Valley, Moonlighter
- **Tone / Art Direction**: Cozy Pixel Art (Sprout Lands / Cainos Asset Pack)
- **Target Platform**: PC (StandaloneWindows64)
- **Screen Orientation / Resolution**: Landscape 1920x1080
- **Render Pipeline**: Universal Render Pipeline (URP 2D com Renderer2DData)

# Game Mechanics
## Core Gameplay Loop
1. **Movimentação e Exploração**: O jogador navega pela padaria utilizando movimentação top-down 2D.
2. **Detecção de Proximidade**: Ao se aproximar de balcões (`Counter`), lixeira (`TrashCan`) ou outros elementos interativos, o sistema detecta o objeto mais próximo e disponibiliza a ação.
3. **Interação com Objetos**: Pressionando a tecla de interação (`E`), o jogador pega ingredientes, processa alimentos ou descarta itens.
4. **Feedback em Tempo Real**: Em vez de mensagens restritas ao console (`Debug.Log`), o jogo exibe caixas de diálogo e mensagens informativas no HUD em tempo real, informando o que foi coletado, descartado ou avisando quando não há espaço/itens.

## Controls and Input Methods
- **Movimentação**: Teclas W, A, S, D ou Setas direcionais (New Input System).
- **Interação**: Tecla E (Keyboard.current.eKey).
- **Avançar / Fechar Diálogo**: Tecla E ou Espaço (ou timeout automático configurável).

# UI
- **Canvas Principal**: Configurado em modo Screen Space - Overlay ou Camera no GameObject `UI`.
- **DialogueBox Panel**:
  - Caixa de diálogo retangular (`DialogueBox` / `Image`) posicionada na parte inferior central da tela.
  - Componente de texto TextMeshPro (`DialogueText` / `TextMeshProUGUI`) com quebra de linha automática e tipografia legível.
  - Comportamento de esmaecimento/desativação automática após tempo limite (ex: 2.5s a 3.5s) ou ao pressionar a tecla de interação.
- **Interaction Prompt (Opcional)**:
  - Indicador visual leve (ex: "Pressione E para interagir") ao entrar no trigger de um `Interactable`.

# Key Asset & Context
- **Cenas Envolvidas**:
  - `Assets/Scenes/Bakery.unity` (Cena da padaria com Grid, Walls, Counters, TrashCan, Chicken).
  - `Assets/Scenes/SampleScene.unity` (Cena base com Player, Main Camera e UI).
- **Scripts Existentes**:
  - `Assets/Scripts/InteractableObjects/Interactable.cs`
  - `Assets/Scripts/InteractableObjects/Counter.cs`
  - `Assets/Scripts/InteractableObjects/TrashCan.cs`
  - `Assets/Scripts/Player/PlayerInteraction.cs`
  - `Assets/Scripts/Player/PlayerInventory.cs`
  - `Assets/Scripts/Enums/Enums.cs`
- **Novos Scripts**:
  - `Assets/Scripts/UI/DialogueManager.cs` (Gerenciador singleton para exibição e controle do diálogo no Canvas).
- **Texturas e Sprites**:
  - `Assets/ImportedAssets/Sprout Lands - Sprites - premium pack/Tilesets/Building parts/Wooden_House_Walls_Tilset.png` (Restaurar pivots para Center).

# Implementation Steps

### Step 1: Correção do Pivot dos Sprites de Parede e Tile Palette
- **Description**: Reverter o alinhamento de todos os sprites fatiados em `Wooden_House_Walls_Tilset.png` de `BottomCenter` volta para `Center (0.5, 0.5)`. Isso restabelece imediatamente o alinhamento correto das grades na Tile Palette (`Indoors.prefab`) e no Tilemap sem que os blocos fiquem deslocados para cima.
- **Assigned role**: developer
- **Dependencies**: None
- **Parallelizable**: Yes

### Step 2: Configuração de Renderização e Sorting do Tilemap Walls
- **Description**: No GameObject `Walls` da cena `Bakery.unity`:
  1. Alterar o `TilemapRenderer.mode` de `Chunk` para `Individual`.
  2. Ajustar a `Sorting Layer` do `TilemapRenderer` para `Instances` (a mesma do Player e dos itens).
  3. Manter `tileAnchor` em `(0.5, 0.5, 0)` com o URP 2D já configurado em `TransparencySortAxis (0, 1, 0)`. Com isso, a profundidade entre o jogador e as paredes funciona dinamicamente pelo eixo Y (frente/costas) sem quebrar as emendas dos blocos.
- **Assigned role**: developer
- **Dependencies**: Depends on Step 1
- **Parallelizable**: No

### Step 3: Criação do DialogueManager
- **Description**: Criar a classe `DialogueManager.cs` em `Assets/Scripts/UI/DialogueManager.cs` contendo:
  - Padrão Singleton seguro (`public static DialogueManager Instance { get; private set; }`).
  - Referências para `GameObject dialogueBox` e `TextMeshProUGUI dialogueText`.
  - Métodos `ShowMessage(string message, float duration = 2.5f)` e `HideDialogue()`.
  - Corotina com cancelamento seguro para ocultar a caixa após a duração especificada.
- **Assigned role**: developer
- **Dependencies**: None
- **Parallelizable**: Yes

### Step 4: Vinculação do DialogueManager na Cena
- **Description**: Adicionar o componente `DialogueManager` ao GameObject `UI` (ou ao `Canvas`) na cena `SampleScene.unity` e conectar as referências existentes de `DialogueBox` e `DialogueText`. Garantir que o `DialogueBox` comece inativo no início do jogo (`Awake`).
- **Assigned role**: developer
- **Dependencies**: Depends on Step 3
- **Parallelizable**: No

### Step 5: Refatoração da Arquitetura de Interação (`Interactable` e Derivados)
- **Description**:
  1. Atualizar `Interactable.cs` para incluir suporte a feedback de diálogo, mantendo suporte polimórfico:
     - Adicionar suporte a mensagem de prompt opcional (ex: `string promptMessage`).
  2. Atualizar `Counter.cs`:
     - Ao pegar um item com sucesso, chamar `DialogueManager.Instance.ShowMessage($"Você pegou: {itemFornecido}!")`.
     - Caso já esteja segurando um item, exibir `DialogueManager.Instance.ShowMessage("Você já está com as mãos ocupadas!")`.
  3. Atualizar `TrashCan.cs`:
     - Ao descartar com sucesso, chamar `DialogueManager.Instance.ShowMessage("O item foi jogado no lixo.")`.
     - Se as mãos estiverem vazias, chamar `DialogueManager.Instance.ShowMessage("Você não tem nada para jogar fora.")`.
  4. Remover ou substituir os `Debug.Log` por chamadas à UI.
- **Assigned role**: developer
- **Dependencies**: Depends on Step 3
- **Parallelizable**: No

### Step 6: Atualização de Detecção no `PlayerInteraction`
- **Description**: Atualizar `PlayerInteraction.cs`:
  - Ao entrar em proximidade (`OnTriggerEnter2D`), exibir mensagem de ajuda na tela (ex: "Pressione [E] para interagir") ou registrar o objeto de forma limpa.
  - Ao sair da proximidade (`OnTriggerExit2D`), limpar o interagível e esconder prompts pendentes.
- **Assigned role**: developer
- **Dependencies**: Depends on Step 4, Step 5
- **Parallelizable**: No

# Verification & Testing
1. **Verificação Visual da Tile Palette**: Abrir a janela de Tile Palette e confirmar que os tiles de `Indoors` e `Walls` estão perfeitamente encaixados na grade retangular 16x16, sem sobreposição nem corte de meio bloco.
2. **Verificação do Y-Sorting das Paredes**: Mover o jogador na cena `Bakery` ao redor das paredes de madeira:
   - Caminhar ao norte das paredes inferiores: o jogador deve ficar atrás da parede.
   - Caminhar ao sul das paredes superiores: o jogador deve ficar na frente da parede.
3. **Verificação da Interação com Balcão (`Counter`)**:
   - Aproximar o jogador com as mãos vazias e pressionar `E`: o balcão deve conceder a farinha e o `DialogueBox` deve aparecer na tela dizendo "Você pegou: Farinha!".
   - Pressionar `E` novamente segurando a farinha: o `DialogueBox` deve alertar "Você já está com as mãos ocupadas!".
4. **Verificação da Interação com a Lixeira (`TrashCan`)**:
   - Aproximar com a farinha na mão e pressionar `E`: o item deve sumir da mão e a mensagem "O item foi jogado no lixo." deve aparecer na tela.
   - Pressionar `E` com as mãos vazias: a mensagem "Você não tem nada para jogar fora." deve aparecer.
5. **Verificação de Desaparecimento do Diálogo**:
   - Confirmar que o balão de diálogo desaparece automaticamente após 2.5 a 3 segundos sem travar o jogo nem gerar erros de `NullReferenceException`.

---

# Current Revision: Finish and Decorate the Bakery Tilemap
This revision extends the existing plan for the user's current request. Implement only the tilemap finishing scope below; earlier dialogue/input/refactoring steps are historical context, not authorization to repeat or expand them. User selected **Cozy balanced polish**: preserve the footprint, finish edges and paths, and add modest clusters of greenery and existing props.

# Project Overview
- Game Title: Bakery 2D (working title from existing plan).
- High-Level Concept: Cozy top-down pixel-art bakery with ingredient collection and counter interactions. This pass makes the existing environment visually complete and readable without introducing new gameplay.
- Players: Single-player, as described in the existing plan.
- Inspiration / Reference Games: Existing plan references Overcooked, Stardew Valley, and Moonlighter; no recreation of their layouts.
- Tone / Art Direction: Cozy Sprout Lands pixel art, matching the sprites already used in Bakery. Prefer existing Sprout Lands assets over the visually different Cainos pack.
- Target Platform: StandaloneWindows64.
- Screen Orientation / Resolution: Landscape; existing plan references 1920x1080. Preserve actual camera framing and reference resolution.
- Render Pipeline: URP, confirmed at runtime through GraphicsSettings.currentRenderPipeline; active asset is Assets/Settings/UniversalRP.asset. Verify its assigned renderer's sorting configuration before edits.

# Game Mechanics
## Core Gameplay Loop
Preserve movement, exploration, ingredient pickup, and disposal. Decoration should guide movement toward the counter and entrance and create recognizable visual landmarks without obscuring stations or reducing usable paths. Under MDA, prioritize cozy aesthetics while maintaining readable movement dynamics. Do not add recipes, interactions, animals, or progression.

## Controls and Input Methods
New Input System is reported active. Retain existing bindings and controller scripts; no input implementation is required. Test the player's actual movement and interaction bindings rather than assuming the historical plan's bindings still apply.

# UI
No UI changes. Preserve the existing HUD and dialogue. Environmental composition, not a new interface, provides guidance.

Conceptual composition (adapt to the existing footprint after visual inspection; not a replacement layout):
```text
[existing boundaries / building walls]
[small greenery cluster]  [existing counter and trash can]
             clear working / movement area
[small accent cluster]    clear approach to entrance
[finished grass / path edges within existing map]
```
Keep foreground foliage from obscuring the player or interaction points unnecessarily. Avoid blanket decoration across every empty cell.

# Key Asset & Context
## Inspected scene state
- Assets/Scenes/Bakery.unity: contains Ground, Walls, Decoration, WalkBehind on a rectangular Grid. Grid transforms are identity and tile anchors are (0.5, 0.5, 0).
- Assets/Scenes/SampleScene.unity: loaded alongside Bakery, containing Player, Main Camera and UI. Preserve these gameplay objects.
- Ground: 434 occupied cells, occupied coordinates (-10,-19) through (12,2). Contains grass variants and 58 Wooden_House_Walls_Tilset_6 tiles; inspect that sprite visually before deciding whether it is a floor tile despite its sheet name.
- Walls: 2361 occupied cells, (-53,-61) through (41,4). Most are Grass_Hill_Tiles_v2 variants, plus five bush tiles and wooden walls. It uses Individual rendering, Objects sorting layer/order 0, Rigidbody2D, CompositeCollider2D and TilemapCollider2D.
- Decoration and WalkBehind: empty, Chunk mode, Default layer/order 0. Default currently sorts above Objects, so blindly painting either layer would put details above characters.
- Sorting layers in ascending order: Background, Walls, Objects, Default. There is no Instances layer; do not follow the historical plan's proposed Instances assignment.
- Existing Player, Counter, TrashCan and chicken renderers use Objects/order 0. Retain the existing interactable instances and animal placement.
- Main Camera reports CustomAxis with axis (0,0,0). Inspect the URP renderer configuration and effective sort behavior before fixing any conflict; do not assume camera output alone proves renderer sorting is broken.
- Inspected Sprout Lands sheets use PPU 16 and Point filtering with mipmaps off. Wooden wall sheet is uncompressed; grass and furniture sheets report compression enabled. Do not change shared importer settings casually.
- Dependency graph for Bakery has been inspected. Before modifying any additional shared asset, inspect its dependencies and dependents.

## Files/assets in scope
- Modify primarily Assets/Scenes/Bakery.unity: tile content, renderer configuration and collision ownership where needed.
- Existing tile assets: Assets/MyTileMaps/Outdoors_Tilemap/Grass_Hill_Tiles_v2_*.asset and Bush_Tiles_*.asset; Assets/MyTileMaps/Indoors_Tilemap/STONE PATH_*.asset, Wooden_House_Walls_Tilset_*.asset, Wooden_House_Roof_Tilset_*.asset and Chest_*.asset as visually appropriate.
- Existing imported sprites under Assets/ImportedAssets/Sprout Lands - Sprites - premium pack/Tilesets/: inspect grass detail sheets, Bush_Tiles.png, Basic_Furniture.png and building parts for suitable accents.
- If an appropriate existing sliced sprite lacks a Tile asset, create only the necessary reusable Tile assets in Assets/MyTileMaps/Decoration_Tilemap/. Resolve sprite references by exact asset and sprite identity after visual inspection, not by guessing numeric suffixes.
- Assets/Settings/UniversalRP.asset and its assigned renderer: inspect; change sorting only if necessary and after impact analysis. No package or shader source changes.
- SampleScene: no planned edits; if an effective scene-local camera sorting correction proves necessary, pause for scope review before changing it.

## Intended layer contract
| Tilemap | Content | Rendering / collision |
|---|---|---|
| Ground | Base grass, floors, flat paths, non-occluding terrain | Background/order 0, Chunk; no blanket collision |
| Decoration | Flat flowers, grass tufts, ground accents and decals | Background/order 1, Chunk; no collider |
| Walls | Actual solid walls and intentional blocking terrain/cliff edges | Preserve Objects/order 0 and Individual where Y-sort is required; existing static composite collision restricted to intended blockers |
| WalkBehind | Bushes and tall static props which must occlude characters according to Y | Objects/order 0, Individual; compatible effective Y axis and sprite sort point; collision only for solid bases |

The GameObject layer and Sorting Layer are separate concerns. Preserve gameplay physics layers unless an existing collision mask requires otherwise. Tall props must not be forced permanently above the player via a high sorting order. Inspect sprite pivots and multi-cell assembly: preserve centered tiled wall pivots for seams; group complete multi-cell props if needed instead of introducing depth artifacts or globally changing sprite pivots. If solid WalkBehind props are added, use appropriately sized base colliders or vetted tile collider geometry; decorative foliage must not block movement across its entire canopy. Preserve intentional perimeter blockers when relocating terrain.

# Implementation Steps
1. **Establish visual baseline and protected areas.** Dependency: approval of this revision.
   - Capture the current 2D scene, inspect the full occupied area and a close-up of the playable bakery; compare the unusually large Walls extent with visible content rather than deleting distant tiles.
   - Inspect the actual sprite sheets to identify connected edge/corner pieces and available decorations.
   - Record tile coordinates, transforms, colors and flags, collision setup, station footprints, entrances and current walkable routes. Preserve user-authored objects and unrelated unsaved work.
   - Verify effective URP renderer sorting and physics masks before adjusting tilemap settings.
2. **Correct tilemap responsibilities without redesigning the level.** Dependency: Step 1.
   - In Bakery, classify grass sprites by visual function: move flat terrain currently on Walls into Ground; retain or relocate true blocking cliff edges based on their intended function.
   - Preserve each moved tile's matrix/color/flags. Do not overwrite an occupied destination silently; resolve visually and preserve intentional stacked content.
   - Move existing bush decoration to WalkBehind when it has height, or Decoration if it is a flat accent. Retain actual wooden walls on Walls. Inspect Wooden_House_Walls_Tilset_6 before treating it as misplaced.
   - Set Decoration and WalkBehind renderers according to the layer contract. Rebuild collision geometry so relocated flat grass does not accidentally remain blocking, while intended boundaries remain solid.
3. **Finish ground and structural seams.** Dependency: Step 2.
   - Repair visible unintended holes, incorrect edge/corner transitions, wall joins and incomplete local paths using matching existing assets.
   - Keep the footprint and intentional openings intact. Do not flood-fill the entire Walls bounding box, expand the map, roof over playable interiors, or turn intentional negative space into terrain.
   - Add or connect short path accents only where they support the existing entrance and station approach. Keep character-width routes clear and preserve their existing clearance.
4. **Add balanced decoration.** Dependency: Step 3.
   - Add a few coherent clusters of low flowers/grass on Decoration near boundaries, leaving central movement spaces quiet.
   - Add modest tall bush/prop accents on WalkBehind at suitable edges. Prefer available bakery-compatible furniture or storage details only when their sprite identity and scale are confirmed.
   - Keep complete multi-tile silhouettes, coherent palette and PPU, avoid repeated stamp patterns, floating props, overlapping wall joins or visual duplicates of working interactables.
   - Create minimal missing Tile assets from existing sprites as necessary; no new art generation or asset downloads.
   - Perform scene edits with Undo support using an editor command with explicit tile/layer placement tables. Reuse and revise the same placement command for fixes; do not install a runtime level generator.
5. **Validate, refine, and save.** Dependency: Steps 2–4.
   - Capture a fresh 2D overview and close-ups; inspect seams, asset scale, overlapping silhouettes and readability.
   - Test movement and front/behind sorting around walls and WalkBehind objects. Test counter and trash interactions and ensure existing animals remain reachable and unblocked.
   - Make no more than three capture-and-fix rounds; obtain feedback if substantial unresolved visual issues remain.
   - Save only the intended scene and newly created tile assets. Report actual changes, validation performed and any outstanding limits.

# Verification & Testing
- Layer audit: Ground contains flat base terrain; Decoration contains only nonblocking flat accents; Walls contains intended blockers; WalkBehind contains actual occluding props. Confirm Decoration stays below Objects regardless of Y.
- Tile integrity: no lost cells outside explicitly corrected areas; tile transforms/colors retained; no accidental overwrite, missing sprite references or broken multi-tile silhouettes.
- Collision: walk across former flat-grass cells on Walls where they are intended to be walkable; confirm solid walls/perimeter boundaries still block. Confirm no canopy-sized accidental obstacle.
- Sorting: move north/south of tall props and front walls, checking that characters pass behind/in front naturally without per-tile fragmentation. Confirm effective axis is nonzero and consistent with the renderer's configuration.
- Gameplay regression: player spawn, entrances and routes to Counter/TrashCan remain usable; interactions and item displays still work; animal behavior is not changed by decorative collision.
- Visual: inspect full map and playable-area screenshots for visible gaps, incorrect corners, prop overlap, oversized furniture, repeated patterns and decoration obscuring station prompts.
- Pixel consistency: match existing 16 PPU sprites; avoid fractional tile offsets, scaling, negative cell gaps or shared pivot changes. Inspect new tile sprites for Point filtering and mipmaps off. If compression visibly harms new details, perform shared-asset impact analysis and request any broader importer fix separately.
- Console: check for new missing references, collider errors, runtime exceptions or compilation errors. Compare against pre-existing errors rather than claiming unrelated errors were fixed.
- Persistence: reopen Bakery after saving and confirm tile/layer settings persist. Confirm SampleScene and shared prefab/controller/package assets remain unchanged unless separately approved.
