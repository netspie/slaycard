"use client";

import { useCardSelectionState } from "@/app/state/CardSelectedState";
import Image from "next/image";
import { forwardRef, useImperativeHandle, useRef } from "react";

import useMouseDrag, { DropElement } from "../core/transform/UseMouseDrag";

export type ActionCardProps = {
  id: number;
  name: string;
  index: number;
  imagePath: string;
  color: string;
  targets?: (DropElement | null)[];
};

export interface ActionCardRef {
  el: HTMLDivElement | undefined;
  props: ActionCardProps;
}

export enum BorderColor {
  Red = "border-red-300",
  Green = "border-green-300",
  Blue = "border-blue-300",
  Yellow = "border-yellow-300",
  White = "border-white-300",
}

export enum BgColor {
  Red = "bg-red-300",
  Green = "bg-green-300",
  Blue = "bg-blue-300",
  Yellow = "bg-yellow-300",
  White = "bg-white",
}

export enum ShadowColor {
  Red = "box-shadow-red",
  Green = "box-shadow-green",
  Blue = "box-shadow-blue",
  Yellow = "box-shadow-yellow",
  White = "box-shadow-white",
}

export enum HueColor {
  Red = "hue-red",
  Green = "hue-green",
  Blue = "hue-blue",
  Yellow = "hue-yellow",
  White = "hue-white",
}

export enum HueBlurColor {
  Red = "hue-red-blur",
  Green = "hue-green-blur",
  Blue = "hue-blue-blur",
  Yellow = "hue-yellow-blur",
  White = "hue-white-blur",
}

export function strToBorderColor(str: string): BorderColor {
  return (
    Object.values(BorderColor).find((c) => c.includes(str)) || BorderColor.Red
  );
}

export function strToBgColor(str: string): BgColor {
  return Object.values(BgColor).find((c) => c.includes(str)) || BgColor.Red;
}

export function strToShadowColor(str: string): ShadowColor {
  return (
    Object.values(ShadowColor).find((c) => c.includes(str)) || ShadowColor.Red
  );
}

export function strToHueColor(str: string): HueColor {
  return Object.values(HueColor).find((c) => c.includes(str)) || HueColor.Red;
}

export function strToHueBlurColor(str: string): HueBlurColor {
  return (
    Object.values(HueBlurColor).find((c) => c.includes(str)) || HueBlurColor.Red
  );
}

type CardBorderProps = {
  borderColor: BorderColor;
  blurColor: BgColor;
  shadowColor: ShadowColor;
};

export function CardBorder(props: CardBorderProps) {
  return (
    <>
      <div
        className={`absolute w-[15%] h-0 ${props.borderColor} border-1 -top-[1px] -left-[1px] ${props.shadowColor}`}
      />
      <div
        className={`absolute w-0 h-[15%] ${props.borderColor} border-1 -top-[1px] -left-[1px] ${props.shadowColor}`}
      />
      <div
        className={`absolute w-[15%] h-0 ${props.borderColor} border-1 -bottom-[1px] -right-[1px] ${props.shadowColor}`}
      />
      <div
        className={`absolute w-0 h-[15%] ${props.borderColor} border-1 -bottom-[1px] -right-[1px] ${props.shadowColor}`}
      />
      <div
        className={`w-1/2 h-1/2 ${props.blurColor} blur-lg opacity-100 pointer-events-none`}
      />
    </>
  );
}

export const ActionCard = forwardRef<ActionCardRef, ActionCardProps>(
  (props, fRef) => {
    const ref = useRef<HTMLDivElement>(null);
    const { isSelected, setSelected } = useCardSelectionState();

    useMouseDrag(ref, props.targets);

    useImperativeHandle(
      fRef,
      () => {
        return {
          el: ref.current as HTMLDivElement,
          props: props,
        };
      },
      [fRef]
    );

    return (
      <>
        <div
          ref={ref}
          className={`relative bg-slate-700 h-[50%] flex flex-col items-center justify-center shadow-md ${strToShadowColor(
            props.color
          )}
          ${
            !isSelected &&
            "cursor-pointer hover:bg-slate-800 active:bg-slate-900"
          }`}
          style={{ aspectRatio: 1 / 1 }}
        >
          {props.imagePath && (
            <Image
              className="absolute w-full h-full -z-10 pointer-events-none"
              // src={`${props.imagePath}`}
              src="/bgs/skill-icon-bg.jpg"
              alt="Dope"
              layout="fill"
              objectFit="cover"
            />
          )}

          <CardBorder
            borderColor={strToBorderColor(props.color)}
            blurColor={strToBgColor(props.color)}
            shadowColor={strToShadowColor(props.color)}
          />

          <div className="absolute w-[75%] h-[75%] pointer-events-none">
            {props.imagePath && (
              <>
                {/* <Image
                  className={`absolute pointer-events-none  opacity-50 ${strToHueBlurColor(props.color)}`}
                  src={`${props.imagePath}`}
                  alt="Dope"
                  layout="fill"
                  objectFit="cover"
                /> */}
                <Image
                  className={`absolute pointer-events-none ${strToHueColor(
                    props.color
                  )}`}
                  src={`${props.imagePath}`}
                  alt="Dope"
                  layout="fill"
                  objectFit="cover"
                />
                {/* <div className="w-full h-full bg-red-500" /> */}
              </>
            )}
          </div>

          <div
            className={`${
              !isSelected &&
              "fixed cursor-pointer bg-slate-800 opacity-0 hover:opacity-40 w-full h-full -z-10"
            }`}
          />
          <span
            className="text-white font-bold w-full text-center"
            style={{ fontSize: "1.3cqh" }}
          >
            {/*   {props.name} */}
          </span>
        </div>
      </>
    );
  }
);

ActionCard.displayName = "ActionCard";
