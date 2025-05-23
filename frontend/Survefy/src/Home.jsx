import * as React from 'react';
import Filters from './myComponents/Filters';
import Note from './myComponents/Note';
import { fetchNotes } from './services/questions';
import { Box, Container } from '@mui/material';
import homeBg from './assets/homeBg.png'; 
import Button from '@mui/material/Button';
import SurvefyBigButton from './myComponents/SurvefyBigButton';
import SurvefyNagivate from './myComponents/SurvefyNavigate';
import Background from './Background';


export default function Home(props) {
  const[notes, setNotes] = React.useState([]);
  const[filter, setFilter] = React.useState({
    search: "",
    sortItem: "date",
    sortOrder: "desc"
  })

  React.useEffect(() => {
    const fetchData = async () => {
      let notes = await fetchNotes(filter);
      setNotes(notes);
    }

    fetchData();
  }, [filter]);


  const onCreate= async (note) => {
    await createQuestion(note);
    let notes = await fetchNotes(filter);
    setNotes(notes);
}

  return (
    <Background>
      <SurvefyNagivate/>
      <Container
        maxWidth="lg"
        component="main"
        sx={{ display: 'flex', flexDirection: 'column', my: '64px', p: 4 }}>
        <Box component="h3" sx={{ mb: 4, fontSize: 36, fontWeight: 600 }}>
          Your Opinion, Your Power
        </Box>
        <Box component="h1" sx={{ maxWidth:"md", mb: 15, fontSize: 26, fontWeight: 400 }}>
        Don't keep your thoughts to yourself — share them! 
        Let’s build a future together, one that brings comfort to us all.
        </Box>
        <SurvefyBigButton title="START NOW" margin={5}/>
        <Box component="h3" sx={{ mb: 4, fontSize: 30, fontWeight: 600 }}>
          Most popular ongoing surveys
        </Box>
      </Container>
    </Background>
    /*
    <Container
            maxWidth="lg"
            component="main"
            sx={{ display: 'flex', flexDirection: 'column', my: '64px', gap: 4 }}
          >
    <div>
      <div>
        <CreateQuestionForm onCreate ={onCreate}/>
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
      </Container>*/
  );
}
