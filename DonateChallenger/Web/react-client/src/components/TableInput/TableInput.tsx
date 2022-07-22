import React from 'react';

interface TableInpuProps {
     value: string | number;
     onChange: React.ChangeEventHandler<HTMLInputElement>;
}

const TableInput = (props: TableInpuProps) => {

     return (
          <input value={props.value} onChange={props.onChange}
               className='color-silver border-bottom bg-transparent w-100' />
     );
};

export default TableInput;