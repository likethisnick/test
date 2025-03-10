import * as React from 'react';
import CssBaseline from '@mui/material/CssBaseline';
import Container from '@mui/material/Container';
import AppTheme from '../shared-theme/AppTheme';
import MainContent from './components/MainContent';
import Latest from './components/Latest';
import Footer from './components/Footer';
import SurvefyNagivate from './myComponents/SurvefyNavigate';
import Background from './Background';
import SurvefyBigButton from './myComponents/SurvefyBigButton';
import { Box } from '@mui/material';
import CreateQuestionForm from './myComponents/createQuestionForm';
import Filters from './myComponents/Filters';
import Note from './myComponents/Note';
import { createQuestion, fetchNotes } from './services/questions';
import { getCurUserId } from './services/getcuruserid';
import CreateSurveyForm from './myComponents/createNewSurvey';
import { createTemplateSurvey } from './services/createTemplateSurvey';


export default function CreateSurvey(props) {
  const[userId, setUserId] = React.useState("'00000000-0000-0000-0000-000000000000'");
  const[notes, setNotes] = React.useState([]);
  const[survey, setSurvey] = React.useState([]);
   const[filter, setFilter] = React.useState({
      search: "",
      sortItem: "date",
      sortOrder: "desc"
    })
    
    const getuserId= async () => {
      getCurUserId().then(response => {
        if(response===undefined){
          return;
        }
        else{
          setUserId(response);
        }
      });
    }
    getuserId();

    const onCreate = async (survey) => {
      const updatedSurvey = {
        ...survey,
        createdbyuserid: userId,
        createdon:  new Date().toISOString()
      };
    
      await createTemplateSurvey(updatedSurvey);
      // let notes = await fetchNotes(filter);
      // setNotes(notes);
    };
    
  return (
    <Background >
      <SurvefyNagivate/>
      <Container
            maxWidth="lg"
            component="main"
            sx={{ display: 'flex', flexDirection: 'column', my: '64px', gap: 4, overflowY: 'auto', }}
          >
    <div>
      <div>
        <CreateSurveyForm onCreate ={onCreate}/>
          <Filters filter={filter} setFilter={setFilter}/>
      </div>
      <ul className="ul">
        <li>
          <Note title="a" description="b"/>
        </li>
          {notes.map((n) => (
            
              <li key={n.id}>
                <ul></ul>
                  <Note title={n.title} description={n.description} createdAt={n.createdAt}/>
              </li>
          ))}
        </ul>
      </div>
      </Container>
    </Background>

    
  );
}
