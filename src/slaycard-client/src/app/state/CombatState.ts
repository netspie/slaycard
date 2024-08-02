import { CardProps } from "@/app/components/Card";
import { create } from "zustand";

const enemyDeck: CardDeckVM = {
  characterCards: [
    { id: 1, name: "Goblin", hp: 6, attack: 3, energy: 5, imageIndex: 3 },
    { id: 2, name: "Imp", hp: 2, attack: 4, energy: 3, imageIndex: 4 },
  ],
};

const playerDeck: CardDeckVM = {
  characterCards: [
    { id: 1, name: "Ellesandra", hp: 6, attack: 3, energy: 5, actionCards: [
        { id: 1, name: "Melee", energyCost: 2, damageFactor: 0.8, imageIndex: 0, color: 'red' },
        { id: 2, name: "Blast", energyCost: 2, damageFactor: 0.8, imageIndex: 1, color: 'green' },
        { id: 3, name: "Fire", energyCost: 2, damageFactor: 0.8, imageIndex: 2, color: 'red' },
        { id: 4, name: "Storm", energyCost: 2, damageFactor: 0.8, imageIndex: 3, color: 'green' },
        { id: 5, name: "Bolt", energyCost: 2, damageFactor: 0.8, imageIndex: 4, color: 'red' },
      ],
      imageIndex: 0
    },
    { id: 2, name: "Garmir", hp: 2, attack: 4, energy: 3, actionCards: [
        { id: 1, name: "Ice", energyCost: 2, damageFactor: 0.8, imageIndex: 5, color: 'red' },
        { id: 2, name: "Fire", energyCost: 2, damageFactor: 0.8, imageIndex: 6, color: 'red' },
        { id: 3, name: "Fire", energyCost: 2, damageFactor: 0.8, imageIndex: 7, color: 'red' },
        { id: 4, name: "Fire", energyCost: 2, damageFactor: 0.8, imageIndex: 4, color: 'yellow' },
        { id: 5, name: "Fire", energyCost: 2, damageFactor: 0.8, imageIndex: 2, color: 'red' },
      ],
      imageIndex: 1
   },
    { id: 3, name: "Tuvial", hp: 2, attack: 4, energy: 3, actionCards: [
      { id: 1, name: "Bolt", energyCost: 2, damageFactor: 0.8, imageIndex: 2, color: 'white' },
      { id: 2, name: "Bolt", energyCost: 2, damageFactor: 0.8, imageIndex: 0, color: 'blue' },
      { id: 3, name: "Bolt", energyCost: 2, damageFactor: 0.8, imageIndex: 1, color: 'blue' },
      { id: 4, name: "Bolt", energyCost: 2, damageFactor: 0.8, imageIndex: 3, color: 'green' },
      { id: 5, name: "Bolt", energyCost: 2, damageFactor: 0.8, imageIndex: 6, color: 'white' },
    ],
    imageIndex: 2
   },
  ],
};

type CombatState = {
  enemyDeck: CardDeckVM;
  playerDeck: CardDeckVM;
  turn: Turn;
  playerTurnState: PlayerTurnState;
  setCombatState: (turn: Turn) => void;
  setSelectedCardForActionState: (card: CardProps) => void;
};

export const useCombatState = create<CombatState>()((set, get) => ({
  enemyDeck: enemyDeck,
  playerDeck: playerDeck,
  turn: Turn.Player,
  playerTurnState: { selectedCardForAction: undefined },
  setCombatState: (turn: Turn) => {
    const state = get();
    state.turn = turn;
    set(state);
  },

  setSelectedCardForActionState: (card: CardProps) => {
    const state = get();
    state.playerTurnState.selectedCardForAction = card;
    set(state);
  },
}));

enum Turn {
  Player,
  Enemy,
}

type PlayerTurnState = {
  selectedCardForAction: CardProps | undefined;
};

type CardVM = {
  id: number;
  name: string;
  hp: number;
  attack: number;
  energy: number;
  actionCards?: ActionCardVM[];
  imageIndex: number
};

type ActionCardVM = {
  id: number;
  name: string;
  energyCost: number;
  damageFactor: number;
  imageIndex: number;
  color: string
};

type CardDeckVM = {
  characterCards: CardVM[];
};
