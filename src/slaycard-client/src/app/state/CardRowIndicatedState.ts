import { create } from "zustand";

type CardRowIndicatedState = {
  hasIndicatedRow: boolean;
  indicationSelectedCardId: number | undefined;
  indicationX: number;
  indicationY: number;
  rowIndex: number;
  setIndicatedRow: (
    hasIndicated: boolean,
    selectedCardId: number | undefined,
    indicationX: number,
    indicationY: number,
    rowIndex: number
  ) => void;
};

export const useCardRowIndicatedState = create<CardRowIndicatedState>()(
  (set) => ({
    hasIndicatedRow: false,
    indicationSelectedCardId: undefined,
    indicationX: 0,
    indicationY: 0,
    rowIndex: 0,
    setIndicatedRow: (
      hasIndicatedRow: boolean,
      indicationSelectedCardId: number | undefined,
      indicationX: number,
      indicationY: number,
      rowIndex: number
    ) =>
      set({
        hasIndicatedRow,
        indicationSelectedCardId,
        indicationX,
        indicationY,
        rowIndex
      }),
  })
);
