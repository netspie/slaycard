type HeroPanelProps = {
  isEnemy: boolean;
};

export default function HeroPanel(props: HeroPanelProps) {
  return (
    <div
      className={`h-full w-full flex ${
        props.isEnemy ? "flex-col-reverse" : "flex-col"
      } pr-10`}
    >
      <div className="relative h-1/3 w-full flex shrink-0 items-center justify-center gap-3 p-5">
        <div
          className="avatar h-[75%] bg-slate-700 flex shrink-0"
          style={{ aspectRatio: 1 / 1 }}
        ></div>
        <div
          className={`w-full h-full flex ${
            props.isEnemy ? "flex-col-reverse" : "flex-col"
          } grow-1`}
        >
          <div className="w-full h-1/2 flex items-center justify-center gap-4">
            <span
              className="w-1/4 font-bold text-right"
              style={{ fontSize: "1.5cqh" }}
            >
              17
            </span>
            <div className="flex w-3/4  gap-1 after:min-w-fit h-full">
              <div className="wins w-full h-full flex gap-1 items-center">
                <div
                  className="win h-[50%] bg-slate-700 flex"
                  style={{ aspectRatio: 1 / 1 }}
                ></div>
                <div
                  className={`win h-[50%] bg-slate-700 flex`}
                  style={{ aspectRatio: 1 / 1 }}
                ></div>
              </div>
            </div>
          </div>
          <div className={`w-full h-1/2 flex flex-col justify-center pl-5`}>
            <span className="font-bold" style={{ fontSize: "1.5cqh" }}>
              Gugolak
            </span>
            <span className="" style={{ fontSize: "1.5cqh" }}>
              Sayains
            </span>
          </div>
        </div>
        <div
            className={`absolute min-w-[10%] bg-slate-800 right-0 ${!props.isEnemy ? 'top-full -translate-y-full' : 'top-0 translate-y-full'} z-1000`}
            style={{ aspectRatio: 1 / 1 }}
          >
            <span
              className="absolute left-[50%] top-[50%] -translate-x-1/2 -translate-y-1/2 text-sm/[12px] font-bold select-none text-white"
              style={{ fontSize: "1.5cqh" }}
            >
              {/* {Math.floor(Math.random() * 10 + 1)} */}
              9
            </span>
        </div>
      </div>
      <div className="h-full w-full grow-1 pr-10">
        <div
          className={`h-full w-full flex shrink-0 items-center justify-center gap-3 p-5`}
        >
          <div
            className={`avatar h-[75%] bg-slate-700 flex shrink-0`}
            style={{ aspectRatio: 1 / 1.5 }}
          ></div>
          <div
            className={`avatar h-[15%] bg-slate-700 flex shrink-0 self-center`}
            style={{ aspectRatio: 1 / 1.1 }}
          ></div>
        </div>
      </div>
    </div>
  );
}
