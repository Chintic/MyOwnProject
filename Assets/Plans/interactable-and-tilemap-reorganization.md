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
