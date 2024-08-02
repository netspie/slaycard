import { CardProps } from "@/app/components/Card";
import { create } from "zustand";

type CardSelectedState = {
  isSelected: boolean
  selectedCard: CardProps | undefined
  setSelected: (isSelected: boolean, selectedCard: CardProps | undefined) => void
}

export const useCardSelectionState = create<CardSelectedState>()((set) => ({
  isSelected: false,
  selectedCard: undefined,
  setSelected: (isSelected: boolean, selectedCard: CardProps | undefined) => set({ isSelected, selectedCard })
}))
