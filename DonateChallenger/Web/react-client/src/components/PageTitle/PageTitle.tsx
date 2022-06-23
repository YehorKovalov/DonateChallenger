import './styles.css';

interface PageTitleProps {
     title: string
}

const PageTitle = (props: PageTitleProps) => {
     return (
          <div className='mb-3'>
               <div className='title display-2'>{props.title}</div>
          </div>
     );
};

export default PageTitle;