"use client";

import { useCardSelectionState } from "@/app/state/CardSelectedState";
import {
  forwardRef,
  useEffect,
  useImperativeHandle,
  useRef,
  useState,
} from "react";
import Image from "next/image";
import { strToBorderColor, strToShadowColor } from "./ActionCard";
import { DropElement } from "../core/transform/UseMouseDrag";
import { isatty } from "tty";

export interface CardRef extends DropElement {
  el: HTMLDivElement | undefined;
  props: CardProps;
}

export type CardProps = {
  id: number;
  name: string;
  hp: number;
  energy: number;
  attack: number;
  isOfPlayer: boolean;
  isSelected: boolean;
  imagePath?: string;
  onSelected?: (card: CardProps) => void;
};

export const Card = forwardRef<CardRef, CardProps>((props, fRef) => {
  const ref = useRef<HTMLDivElement>(null);
  const [isSelectedLocal, setSelectedLocal] = useState(false);
  const { isSelected, setSelected } = useCardSelectionState();
  const [isArtifactOver, setArtifactOver] = useState(false);

  useImperativeHandle(
    fRef,
    () => {
      return {
        el: ref.current as HTMLDivElement,
        props: props,
        ref: ref,
        onDragEnter: () => setArtifactOver(true),
        onDragExit: () => setArtifactOver(false),
      };
    },
    [fRef]
  );

  useEffect(() => {}, [isArtifactOver]);

  const getBorderMarkColor = (isSelected: boolean) => {
    return isSelected ? strToBorderColor("blue") : strToBorderColor("white");
  };

  const getBorderMarkShadow = (isSelected: boolean) => {
    return isSelected ? strToShadowColor("blue") : strToShadowColor("white");
  };

  return (
    <div
      ref={ref}
      onClick={() => {
        props.onSelected && props.onSelected(props);
      }}
      className={`character relative bg-slate-700 h-full flex flex-col items-center shadow-2xl 
        ${isSelectedLocal && "z-10"} ${
          (props.isSelected ||
            !isSelected ||
            (isSelected && !props.isOfPlayer)) &&
          "cursor-pointer hover:bg-slate-800 active:bg-slate-900"
        }
        ${
          (props.isSelected && strToShadowColor("blue")) ||
          strToShadowColor("white")
        }`}
      style={{ aspectRatio: 1 / 1.5 }}
    >
      <div className="absolute w-full h-full overflow-clip flex flex-col justify-center items-center">
        {props.imagePath && (
          <Image
            className="absolute bg-repeat-y h-full w-full top-0 pointer-events-none"
            src="/bgs/skill-icon-bg.jpg"
            alt="Dope"
            layout="fill"
            objectFit="cover"
          />
        )}
        <div
          className={`w-1/2 h-1/2 bg-white blur-lg opacity-100 pointer-events-none`}
        />
        {props.imagePath && (
          <Image
            className="absolute" // hue-white scale-[175%]
            src={`${props.imagePath}`}
            alt="Dope"
            layout="fill"
            unoptimized
            style={{ top: "0%" }}
          />
        )}

        <div className="absolute bottom-0 w-full h-[50%] bg-gradient-to-t from-black to-transparent" />
      </div>

      <div
        className={`absolute w-[15%] h-0 ${getBorderMarkColor(
          props.isSelected
        )} border-1 -top-[1px] -left-[1px] ${getBorderMarkShadow(
          props.isSelected
        )}`}
      />
      <div
        className={`absolute w-0 h-[10%] ${getBorderMarkColor(
          props.isSelected
        )} border-1 -top-[1px] -left-[1px] ${getBorderMarkShadow(
          props.isSelected
        )}`}
      />
      <div
        className={`absolute w-[15%] h-0 ${getBorderMarkColor(
          props.isSelected
        )} border-1 -bottom-[1px] -right-[1px] ${getBorderMarkShadow(
          props.isSelected
        )}`}
      />
      <div
        className={`absolute w-0 h-[10%] ${getBorderMarkColor(
          props.isSelected
        )} border-1 -bottom-[1px] -right-[1px] ${getBorderMarkShadow(
          props.isSelected
        )}`}
      />
      <div
        className={`absolute w-[15%] h-0 ${getBorderMarkColor(
          props.isSelected
        )} border-1 -top-[1px] -right-[1px] ${getBorderMarkShadow(
          props.isSelected
        )}`}
      />
      <div
        className={`absolute w-0 h-[10%] ${getBorderMarkColor(
          props.isSelected
        )} border-1 -top-[1px] -right-[1px] ${getBorderMarkShadow(
          props.isSelected
        )}`}
      />
      <div
        className={`absolute w-[15%] h-0 ${getBorderMarkColor(
          props.isSelected
        )} border-1 -bottom-[1px] -left-[1px] ${getBorderMarkShadow(
          props.isSelected
        )}`}
      />
      <div
        className={`absolute w-0 h-[10%] ${getBorderMarkColor(
          props.isSelected
        )} border-1 -bottom-[1px] -left-[1px] ${getBorderMarkShadow(
          props.isSelected
        )}`}
      />

      <div
        className="hp absolute top-[3%] left-[6%] w-[20%] flex justify-center items-center bg-red-500 bg-opacity-50 box-shadow-red"
        style={{ aspectRatio: 1 / 1 }}
      >
        <span
          className="absolute text-red-100 font-bold h-full font-['arial']"
          style={{ fontSize: "1.5cqh" }}
        >
          {props.hp}
        </span>
      </div>
      <div
        className="hp absolute top-[3%] right-[6%] w-[20%] flex justify-center items-center bg-green-500 bg-opacity-50 box-shadow-green"
        style={{ aspectRatio: 1 / 1 }}
      >
        <span
          className="absolute text-green-100 font-bold h-full font-['arial']"
          style={{ fontSize: "1.5cqh" }}
        >
          {props.attack}
        </span>
      </div>

      <h6
        className="absolute text-white font-bold w-full text-center"
        style={{ bottom: "0.5cqh", fontSize: "1.5cqh", lineHeight: "1.5cqh" }}
      >
        {props.name}
      </h6>
      {isArtifactOver && (
        <div className="hover-effect w-full h-full bg-white opacity-20" />
      )}
    </div>
  );
});

Card.displayName = "Card";
