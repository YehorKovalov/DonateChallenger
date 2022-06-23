import { Container, interfaces } from "inversify";
import React, { useContext, useMemo } from "react";

const InversifyContext = React.createContext<{ container: Container | null}>({container: null});

type Props = {
     container: Container,
     children?: JSX.Element
}

export const IoCProvider: React.FunctionComponent<Props> = (props: Props) => {
     return (
          <InversifyContext.Provider value={{container: props.container}}>
               {props?.children }
          </InversifyContext.Provider>
     );
};

export function useInjection<T>(identifier: interfaces.ServiceIdentifier<T>): T {
     const { container } = useContext(InversifyContext);
     if (!container) {
       throw new Error();
     }
     
     return useMemo(() => container.get<T>(identifier), [container, identifier]);
};