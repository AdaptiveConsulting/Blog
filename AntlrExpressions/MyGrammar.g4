grammar MyGrammar;  
/* * Parser Rules */  
number: INT | FLOAT;  
fromUomCode: NAME | IDENTIFIER;  
toUomCode: NAME | IDENTIFIER;  
fxRateFunc: 'FXRate' '(' currencyPair ')';  
currencyPair: NAME;  
uomConvertFunc: 'UomConvert' '(' fromUomCode ',' toUomCode ')'; 
expr: expr op=(MUL | DIV) expr #mulDiv
    | expr op=(ADD | SUB) expr #addSub
    | number                   #num
    | '(' expr ')'             #parens
    | fxRateFunc               #fxRate
    | uomConvertFunc           #uomFactor
    ;
    
/* * Lexer Rules */  
fragment DIGIT: [0-9];
fragment LETTER: [a-zA-Z];  
INT: DIGIT + ;
FLOAT: DIGIT + '.'  
DIGIT + ;  
STRING_LITERAL: '\''.* ? '\'';  
NAME: LETTER(LETTER | DIGIT)* ;  
IDENTIFIER: [a-zA-Z0-9]+ ;

MUL: '*';  
DIV: '/';  
ADD: '+';  
SUB: '-';  
WS: [\t\r\n] + -> skip;