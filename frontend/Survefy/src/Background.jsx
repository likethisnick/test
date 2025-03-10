import * as React from 'react';
import CreateQuestionForm from './myComponents/createQuestionForm';
import Note from './myComponents/Note';
import { fetchNotes } from './services/questions';
import { Box, Container } from '@mui/material';
import homeBg from './assets/homeBg.png'; 
import Button from '@mui/material/Button';
import SurvefyBigButton from './myComponents/SurvefyBigButton';
import SurvefyNagivate from './myComponents/SurvefyNavigate';


export default function Background(props) {
  return (
    <Box
      sx={{
        position: 'fixed',
        inset: 0,
        background: 'linear-gradient(to bottom, #f0f0f0,rgba(10, 14, 255, 0.15))',
        overflow: 'auto',
        zIndex: 1,
      }}
    >
        {props.children}
      <Box
        component="img"
        maxWidth="lg"
        src={homeBg}
        sx={{
          position: 'fixed',
          bottom: 0,
          right: 125,
          width: '35%',
          height: 'auto',
          zIndex: -1,
        }}
      />
    </Box>
  );
}
